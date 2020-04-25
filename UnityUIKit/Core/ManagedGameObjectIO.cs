using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using UnityEngineInternal;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;
using YamlDotNet.Serialization.ObjectGraphTraversalStrategies;
using YamlDotNet.Serialization.TypeInspectors;
using YamlDotNet.Serialization.TypeResolvers;

namespace UnityUIKit.Core
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public sealed class YamlSerializableAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class YamlOnlySerializeSerializableAttribute : Attribute
	{
	}

	public static class ManagedGameObjectIO
	{
#if DEBUG
		public static DebugLogger debugLogger = new DebugLogger(Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"UnityUIKit.Debug.yml"), append: false);
#endif

		private static ISerializer m_Serializer;
		private static IDeserializer m_Deserializer;

		static ManagedGameObjectIO()
		{
			var serializer_ = new SerializerBuilder();

			var typeInspectorFactories_ = serializer_.GetType().GetField("typeInspectorFactories", BindingFlags.NonPublic | BindingFlags.Instance);
			var typeInspectorFactories = typeInspectorFactories_.GetValue(serializer_);


			var Add = typeInspectorFactories.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
			Add.Invoke(typeInspectorFactories, new object[] {
				typeof(YamlAttributesTypeInspector),
				new Func<ITypeInspector, ITypeInspector>((ITypeInspector inner) => new YamlAttributesTypeInspector(inner))
			});

			var Remove = typeInspectorFactories.GetType().GetMethod("Remove", BindingFlags.Public | BindingFlags.Instance);
			Remove.Invoke(typeInspectorFactories, new object[] {
				typeof(YamlDotNet.Serialization.YamlAttributesTypeInspector)
			});

#if DEBUG
			var _namingConvention = serializer_.GetType().GetField("namingConvention", BindingFlags.NonPublic | BindingFlags.Instance);
			var namingConvention = _namingConvention.GetValue(serializer_) as INamingConvention;
			var objectGraphTraversalStrategyFactory = (ObjectGraphTraversalStrategyFactory)((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new Debug_TraversalStrategy(typeInspector, typeResolver, maximumRecursion, namingConvention));
			serializer_.WithObjectGraphTraversalStrategyFactory(objectGraphTraversalStrategyFactory);
#endif

			m_Serializer = serializer_.Build();

			var deserializer_ = new DeserializerBuilder();
			var BuildTypeInspector = deserializer_.GetType().GetMethod("BuildTypeInspector", BindingFlags.NonPublic | BindingFlags.Instance);
			deserializer_.WithNodeDeserializer(new ManagedGameObject_Deserializer(BuildTypeInspector.Invoke(deserializer_, new object[] { }) as ITypeInspector));
			m_Deserializer = deserializer_.Build();
		}


		public sealed class YamlAttributesTypeInspector : TypeInspectorSkeleton
		{
			private readonly ITypeInspector innerTypeDescriptor;

			public YamlAttributesTypeInspector(ITypeInspector innerTypeDescriptor)
			{
				this.innerTypeDescriptor = innerTypeDescriptor;
			}

			public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
			{
				var i_result = new List<IPropertyDescriptor>();

				if (type.GetCustomAttribute<YamlOnlySerializeSerializableAttribute>() != null)
				{
					i_result = (from p in (from p in innerTypeDescriptor.GetProperties(type, container)
										   where p.GetCustomAttribute<YamlSerializableAttribute>() != null && p.GetCustomAttribute<YamlIgnoreAttribute>() == null
										   select p).Select((Func<IPropertyDescriptor, IPropertyDescriptor>)delegate (IPropertyDescriptor p)
										   {
											   PropertyDescriptor propertyDescriptor = new PropertyDescriptor(p);
											   YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
											   if (customAttribute != null)
											   {
												   if (customAttribute.SerializeAs != null)
													   propertyDescriptor.TypeOverride = customAttribute.SerializeAs;
												   propertyDescriptor.Order = customAttribute.Order;
												   propertyDescriptor.ScalarStyle = customAttribute.ScalarStyle;
												   if (customAttribute.Alias != null)
													   propertyDescriptor.Name = customAttribute.Alias;
											   }
											   return propertyDescriptor;
										   })
								orderby p.Order
								select p).ToList();
				}
				else
				{
					i_result = (from p in (from p in innerTypeDescriptor.GetProperties(type, container)
										   where p.GetCustomAttribute<YamlIgnoreAttribute>() == null
										   select p).Select((Func<IPropertyDescriptor, IPropertyDescriptor>)delegate (IPropertyDescriptor p)
										   {
											   PropertyDescriptor propertyDescriptor = new PropertyDescriptor(p);
											   YamlMemberAttribute customAttribute = p.GetCustomAttribute<YamlMemberAttribute>();
											   if (customAttribute != null)
											   {
												   if (customAttribute.SerializeAs != null)
													   propertyDescriptor.TypeOverride = customAttribute.SerializeAs;
												   propertyDescriptor.Order = customAttribute.Order;
												   propertyDescriptor.ScalarStyle = customAttribute.ScalarStyle;
												   if (customAttribute.Alias != null)
													   propertyDescriptor.Name = customAttribute.Alias;
											   }
											   return propertyDescriptor;
										   })
								orderby p.Order
								select p).ToList();
				}

				return i_result;
			}

		}


		public class ManagedGameObject_Deserializer : INodeDeserializer
		{

			public static Type TypeByName(string name)
			{
				Type type = Type.GetType(name, throwOnError: false);
				if (type == null)
					type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly x) => x.GetTypes()).FirstOrDefault((Type x) => x.FullName == name);
				if (type == null)
					type = AppDomain.CurrentDomain.GetAssemblies().SelectMany((Assembly x) => x.GetTypes()).FirstOrDefault((Type x) => x.Name == name);
				return type;
			}


			private readonly ITypeInspector typeDescriptor;
			private readonly IObjectFactory objectFactory;
			public ManagedGameObject_Deserializer(ITypeInspector typeDescriptor, IObjectFactory objectFactory = null)
			{
				this.typeDescriptor = (typeDescriptor ?? throw new ArgumentNullException("typeDescriptor"));
				this.objectFactory = (objectFactory ?? new YamlDotNet.Serialization.ObjectFactories.DefaultObjectFactory());
			}

			public bool Deserialize(IParser parser, Type expectedType, Func<IParser, Type, object> nestedObjectDeserializer, out object value)
			{
				value = null;
				if (!typeof(ManagedGameObject).IsAssignableFrom(expectedType) || !parser.TryConsume(out MappingStart _))
				{
					return false;
				}
				bool findType = false;

				MappingEnd event2;

				Scalar scalar = parser.Consume<Scalar>();
				if (scalar.Value == "MGO.Type")
				{
					string i = nestedObjectDeserializer(parser, typeof(string)) as string;
					expectedType = TypeByName(i);
#if DEBUG
					debugLogger.Start("Deserialize");

					debugLogger.WriteLine($"nestedObjectDeserializer : { i }");
					debugLogger.WriteLine($"expectedType : { expectedType }");

					debugLogger.End();
#endif
					if (expectedType == null)
					{
						findType = false;
					}
					else
					{
						value = objectFactory.Create(expectedType);
						findType = true;
					}
				}

				while (!parser.TryConsume(out event2))
				{
					scalar = parser.Consume<Scalar>();
					IPropertyDescriptor property = typeDescriptor.GetProperty(expectedType, null, scalar.Value, false);
					if (property == null || !findType)
					{
						parser.SkipThisAndNestedEvents();
						continue;
					}

					object obj = nestedObjectDeserializer(parser, property.Type);
					IValuePromise valuePromise = obj as IValuePromise;
					if (valuePromise != null)
					{
						object valueRef = value;
						valuePromise.ValueAvailable += (Action<object>)delegate (object v)
						{
							object value3 = YamlDotNet.Serialization.Utilities.
								TypeConverter.ChangeType(v, property.Type);
							property.Write(valueRef, value3);
						};
					}
					else
					{
						object value2 = YamlDotNet.Serialization.Utilities.
							TypeConverter.ChangeType(obj, property.Type);
						property.Write(value, value2);
					}
				}
				return true;
			}
		}


        #region Debug_TraversalStrategy
#if DEBUG
		public class Debug_TraversalStrategy : FullObjectGraphTraversalStrategy
		{
			public Debug_TraversalStrategy(ITypeInspector typeDescriptor, ITypeResolver typeResolver, int maxRecursion, INamingConvention namingConvention)
				: base(typeDescriptor, typeResolver, maxRecursion, namingConvention)
			{
			}

			protected override void Traverse<TContext>(object name, IObjectDescriptor value, IObjectGraphVisitor<TContext> visitor, TContext context, Stack<ObjectPathSegment> path)
			{
				debugLogger.Start("Traverse_" + name);

				debugLogger.ShowStart("value");
				debugLogger.WriteLine($"value.ScalarStyle : {value?.ScalarStyle}");
				debugLogger.WriteLine($"value.StaticType : {value?.StaticType}");
				debugLogger.WriteLine($"value.Type : {value?.Type}");
				debugLogger.WriteLine($"value.Value : {value?.Value}");
				debugLogger.End();

				debugLogger.ShowStart("context");
				debugLogger.WriteLine($"context : {context}");
				debugLogger.End();

				debugLogger.ShowStart("path");
				var i_path = path?.ToList();
				if (i_path != null)
					for(int i = 0;i< i_path.Count;i++)
					{
						debugLogger.ShowStart($"path[{i}]");
						debugLogger.WriteLine($"path[{i}].name : {i_path[i].name}");
						debugLogger.WriteLine($"path[{i}].value : {i_path[i].value}");
						debugLogger.End();
					}
				debugLogger.End();


				debugLogger.End();
				base.Traverse<TContext>(name, value, visitor, context, path);
			}
		}
#endif
		#endregion

		#region DebugLogger
#if DEBUG

		public class DebugLogger : StreamWriter
		{
			private Stack<KeyValuePair<string,bool>> NameStack = new Stack<KeyValuePair<string, bool>>();

			public DebugLogger(string path) : this(path, false)
			{
			}

			public DebugLogger(string path, bool append) : base(path, append)
			{
			}

			public DebugLogger(string path, bool append, Encoding encoding) : this(path, append, encoding, 1024)
			{
			}

			[SecuritySafeCritical]
			public DebugLogger(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
			{
			}

			public override void WriteLine(string text)
			{
				for (int i = 0; i < NameStack.Count; i++)
					text = " " + text;
				base.WriteLine(text);
				debugLogger.Flush();
			}

			public void Start(string name)
			{
				base.WriteLine();
				for (int i = 0; i < NameStack.Count; i++)
					name = " " + name;
				base.WriteLine(name + "_Start");
				NameStack.Push(new KeyValuePair<string, bool>(name, true));
				debugLogger.Flush();
			}

			public void End()
			{
				var tempKV = NameStack.Pop();
				if (tempKV.Value)
					base.WriteLine(tempKV.Key + "_End");
				debugLogger.Flush();
			}

			public void ShowStart(string name)
			{
				base.WriteLine();
				for (int i = 0; i < NameStack.Count; i++)
					name = " " + name;
				base.WriteLine(name + ":");
				NameStack.Push(new KeyValuePair<string, bool>(name, false));
				debugLogger.Flush();
			}
		}

#endif
        #endregion


        public static T Load<T>(string input)
        {
#if DEBUG
			debugLogger.Start("Load");
			var i = default(T);
			i = m_Deserializer.Deserialize<T>(input);
			debugLogger.End();
			return i;
#endif
			return m_Deserializer.Deserialize<T>(input);
		}

        public static string Save(ManagedGameObject input)
        {
#if DEBUG
			debugLogger.Start("Save");
			string i = m_Serializer.Serialize(input);
			debugLogger.End();
			return i;
#endif
			return m_Serializer.Serialize(input);

		}
	}
}
