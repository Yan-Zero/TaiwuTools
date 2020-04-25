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
		static DebugLogger writer = File.CreateText(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "test.yml")) as DebugLogger;
#endif

		private static ISerializer m_Serializer;
		private static IDeserializer m_Deserializer;

		static ManagedGameObjectIO()
		{
			var serializer_ = new SerializerBuilder();

			var typeInspectorFactories_ = serializer_.GetType().GetField("typeInspectorFactories", BindingFlags.NonPublic | BindingFlags.Instance);
			var typeInspectorFactories = typeInspectorFactories_.GetValue(serializer_);

			//var _namingConvention = serializer_.GetType().GetField("namingConvention", BindingFlags.NonPublic | BindingFlags.Instance);
			//var namingConvention = _namingConvention.GetValue(serializer_) as INamingConvention;

			var Add = typeInspectorFactories.GetType().GetMethod("Add", BindingFlags.Public | BindingFlags.Instance);
			Add.Invoke(typeInspectorFactories, new object[] {
				typeof(YamlAttributesTypeInspector),
				new Func<ITypeInspector, ITypeInspector>((ITypeInspector inner) => new YamlAttributesTypeInspector(inner))
			});

			var Remove = typeInspectorFactories.GetType().GetMethod("Remove", BindingFlags.Public | BindingFlags.Instance);
			Remove.Invoke(typeInspectorFactories, new object[] {
				typeof(YamlDotNet.Serialization.YamlAttributesTypeInspector)
			});

			//var objectGraphTraversalStrategyFactory = (ObjectGraphTraversalStrategyFactory)((ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion) => new ManagedGameObject_TraversalStrategy(typeInspector, typeResolver, maximumRecursion, namingConvention));
			//serializer_.WithObjectGraphTraversalStrategyFactory(objectGraphTraversalStrategyFactory);

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
				//if (type.IsAssignableFrom(type))
				//	;
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
					writer.Start("Deserialize");

					writer.WriteLine($"nestedObjectDeserializer : { i }");
					writer.WriteLine($"expectedType : { expectedType }");

					writer.End();
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


        #region DebugLogger
#if DEBUG

        public class DebugLogger : StreamWriter
		{
			private Stack<string> NameStack = new Stack<string>();

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
			}

			public void Start(string name)
			{
				for (int i = 0; i < NameStack.Count; i++)
					name = " " + name;
				base.WriteLine(name + "_Start");
				NameStack.Push(name);
			}

			public void End()
			{
				var name = NameStack.Pop();
				base.WriteLine(name + "_End");
				writer.Flush();
			}
		}

#endif
        #endregion


        public static T Load<T>(string input)
        {
#if DEBUG
			writer.Start("Load");
			var i = default(T);
			i = m_Deserializer.Deserialize<T>(input);
			writer.End();
			return i;
#endif
			return m_Deserializer.Deserialize<T>(input);
		}

        public static string Save(ManagedGameObject input)
        {
#if DEBUG
			writer.Start("Save");
			string i = m_Serializer.Serialize(input);
			writer.End();
			return i;
#endif
			return m_Serializer.Serialize(input);

		}
	}
}
