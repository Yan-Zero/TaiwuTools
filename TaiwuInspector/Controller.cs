using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityAssetsReader;
using System.IO;
using System.Collections;
using System;
using UnityUIKit.GameObjects;
using UnityUIKit.Components;
using TaiwuUIKit.GameObjects;
using UnityUIKit.Core;

namespace TaiwuInspector
{
    class Controller : MonoBehaviour
    {
        private static GameObject GameObjectInstance;
        private int opacity = 0;
        private string debugText;

        private string indexingMessage;
        private Queue<string> indexQueue;
        private Dictionary<string, Type> resourceIndex;
        private Dictionary<Type, int> resourceStats;

        private Container.CanvasContainer overlay;
        private Container frame;
        private Container.ScrollContainer scroll;

        private BoxGroup.ComponentAttributes boxModelArgs;

        protected void Awake()
        {
            overlay = new Container.CanvasContainer()
            {
                Name = "VizCanvas",
                Group =
                {
                    Padding = { 80 },
                },
                Children =
                {
                    (frame = new BaseFrame()
                    {
                        Name = "VizFrame",
                        Children =
                        {
                            (scroll = new BaseScroll()
                            {
                                Name = "ResourceList",
                                Group = {
                                    // FIXME: horizontal doesn't work
                                    //Direction = Direction.Horizontal,
                                    ChildrenAlignment = TextAnchor.UpperLeft,
                                },
                                Element =
                                {
                                    PreferredSize = { 500, 0 },
                                },
                            }),
                            new BaseFrame()
                            {
                                Name = "ResourceInfo",
                            },
                        }
                    }),
                }
            };
        }

        private void Debug()
        {
            IndexResources();

            return;

            //debugText = "";
            //var label = GameObject.Find("StartMenuButton");
            //var image = label.GetComponent<Image>();
            //debugText += $"\n{image.sprite}";
            //var text = label.transform.GetChild(1).gameObject.GetComponent<Text>();
            //debugText += $"\n{text.fontSize}";
            //var level = 0;
            //debugText += "\n" + Dump(text.transform, ref level);

            // FIXME: use a resource loader
            var mainmenu = Resources.Load<GameObject>("oldsceneprefabs/mianmenuback");
            var level = 0;
            UMMShim.Logger.Log("=================== WALK BEGIN");
            UMMShim.Logger.Log(Walk(mainmenu, level));
            UMMShim.Logger.Log("=================== WALK END");
            //debugText = Walk(mainmenu, level);

            return;

            //boxModelArgs.Direction = boxModelArgs.Direction == Direction.Horizontal
            //    ? Direction.Vertical
            //    : Direction.Horizontal;
            //frame.BoxModel.Apply(boxModelArgs);
        }

        private void IndexResources()
        {
            if (indexQueue != null) return;
            indexQueue = new Queue<string>();
            resourceIndex = new Dictionary<string, Type>();
            resourceStats = new Dictionary<Type, int>();

            var assetPath = Path.Combine(Application.dataPath, "globalgamemanagers");
            var globalGameManagersAssetsFile = new GlobalGameManagersAssetsFile(assetPath);
            foreach (var path in new HashSet<string>(globalGameManagersAssetsFile.ResourceList))
            {
                resourceIndex[path] = null;
                indexQueue.Enqueue(path);

                var prefix = "";
                var p = path;
                var level = 0;
                while (true)
                {
                    var index = p.IndexOf('/');
                    if (index == -1)
                    {
                        if (!scroll.Contains(path))
                        {
                            var leafItem = new BaseButton()
                            {
                                Name = $"LeafItem:{path}",
                                Text = $"{string.Concat(Enumerable.Repeat("    ", level))}* {p}",
                                Alignment = HorizontalAnchor.Left,
                                UseBoldFont = false,
                                Color = Color.white,
                            };

                            scroll.Add(path, leafItem);
                        }

                        break;
                    }

                    var segment = p.Substring(0, index);
                    prefix += segment;

                    if (!scroll.Contains(prefix))
                    {
                        var directoryItem = new BaseButton()
                        {
                            Name = $"DirectoryItem:{prefix}",
                            Text = $"{string.Concat(Enumerable.Repeat("    ", level))}- {segment}",
                            Alignment = HorizontalAnchor.Left,
                            UseBoldFont = true,
                        };

                        scroll.Add(prefix, directoryItem);
                    }

                    p = p.Substring(index + 1);
                    level++;
                }
            }
            StartCoroutine(LoadResource());
        }

        IEnumerator LoadResource()
        {
            if (indexQueue.Count == 0)
            {
                indexingMessage = $"已完成对 {resourceIndex.Count} 份资源的检索！";
                yield break;
            }

            var path = indexQueue.Dequeue();
            var request = Resources.LoadAsync(path);
            while (!request.isDone)
            {
                //Main.Logger.Log($"DEBUG: Loading {path}...");
                indexingMessage = $"[{resourceIndex.Count() - indexQueue.Count()}/{resourceIndex.Count()}] 正在检索 {path}...";
                yield return null;
            }

            var resource = request.asset;
            var type = resource.GetType();

            resourceIndex[path] = type;
            if (!resourceStats.ContainsKey(type)) resourceStats[type] = 0;
            resourceStats[type]++;

            //Main.Logger.Log($"{path}: {resource.name} - {resource.GetType().FullName}");
            StartCoroutine(LoadResource());
        }

        private int DebugCount(GameObject go, HashSet<GameObject> set = null)
        {
            if (set == null) set = new HashSet<GameObject>();
            if (set.Contains(go)) return 0;
            set.Add(go);

            var count = 1;
            for (var i = 0; i < go.transform.childCount; i++)
            {
                count += DebugCount(go.transform.GetChild(i).gameObject, set);
            }
            return count;
        }

        protected void OnGUI()
        {
            if (opacity == 0) return;
            GUI.color = new Color(1, 1, 1, opacity / 10f);

            GUIStyle borderStyle = new GUIStyle();
            borderStyle.normal.background = Texture2D.whiteTexture;

            // styles
            GUIStyle boxStyle = new GUIStyle("box")
            {
                fixedWidth = Screen.width,
                fixedHeight = Screen.height,
            };
            GUIStyle valueStyle = new GUIStyle("label");
            valueStyle.normal.textColor = Color.red;

            // get mouse position
            var mousePosition = Camera.current.ScreenToWorldPoint(Input.mousePosition);

            // get game object under mouse
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
                position = Input.mousePosition,
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            var result = results.FirstOrDefault();
            var level = 0;
            var resultOutput = "<nothing>";
            if (results.Count() > 0)
            {
                resultOutput = Dump(result.gameObject.transform, ref level);
                Vector3[] corners = new Vector3[4];
                (result.gameObject.transform as RectTransform).GetWorldCorners(corners);
                var minX = Mathf.Min(corners[0].x, corners[2].x);
                var minY = Mathf.Min(corners[0].y, corners[2].y);
                var maxX = Mathf.Max(corners[0].x, corners[2].x);
                var maxY = Mathf.Max(corners[0].y, corners[2].y);
                var minScreenPos = Camera.current.WorldToScreenPoint(new Vector3(minX, minY, 100));
                var maxScreenPos = Camera.current.WorldToScreenPoint(new Vector3(maxX, maxY, 100));
                var guiPos = new Vector2(minScreenPos.x, Screen.height - maxScreenPos.y);
                var screenSize = maxScreenPos - minScreenPos;
                var guiSize = new Vector2(Mathf.Abs(screenSize.x), Mathf.Abs(screenSize.y));
                var rect = new Rect(guiPos, guiSize);
                //GUI.Box(rect, GUIContent.none, borderStyle); // Draw box
            }

            var uiRoot = GameObject.Find("/UIRoot");
            var goCount = DebugCount(uiRoot);
            var allGoCount = FindObjectsOfType<GameObject>().Count();

            var resourceStatsOutput = resourceStats == null ? "" : string.Join("", resourceStats.OrderBy(pair => pair.Key.FullName).Select(pair => $"\n{pair.Key.FullName}: {pair.Value}"));

            GUILayout.BeginVertical(boxStyle);
            GUILayout.Label("资源统计：");
            GUILayout.Label(indexingMessage + resourceStatsOutput, valueStyle);
            GUILayout.Label("鼠标位置：");
            GUILayout.Label($"({mousePosition.x:0.0},{mousePosition.y:0.0})", valueStyle); // FIXME: use format helper for vec3
            GUILayout.Label($"当前 GameObjects 总数：UIRoot 中 {goCount} 个 / 总共 {allGoCount} 个");
            GUILayout.Label($"当前鼠标下的 GameObjects (共 {level} 层)：");
            GUILayout.Label(resultOutput, valueStyle);
            GUILayout.Label("调试输出：");
            GUILayout.Label(debugText, valueStyle);
            GUILayout.EndVertical();
        }

        private static string Walk(GameObject gameObject, int level)
        {
            var tf = gameObject.transform;
            var components = tf.GetComponents<Component>();
            var gameBehaviours = components.Where(comp => comp is MonoBehaviour && comp.GetType().Namespace == null).ToArray();
            var gameBehavioursOutput = string.Join(",", gameBehaviours.Select(comp => comp.GetType().Name));
            var builtinBehaviours = components.Where(comp => comp is MonoBehaviour && comp.GetType().Namespace != null).ToArray();
            var builtinBehavioursOutput = string.Join(",", builtinBehaviours.Select(comp => comp.GetType().Name));
            var otherComponents = components.Where(comp => !(comp is MonoBehaviour)).ToArray();
            var otherComponentsOutput = string.Join(",", otherComponents.Select(comp => comp.GetType().Name));

            var output = $"\n{string.Concat(Enumerable.Repeat("  ", level))}- {tf.GetSiblingIndex()}:{tf.name}";
            if (gameBehaviours.Length > 0) output += $" game=[{gameBehavioursOutput}]";
            if (builtinBehaviours.Length > 0) output += $" builtin=[{builtinBehavioursOutput}]";
            if (otherComponents.Length > 0) output += $" components=[{otherComponentsOutput}]";
            for (var i = 0; i < tf.childCount; i++)
            {
                var child = tf.GetChild(i).gameObject;
                output += Walk(child, level + 1);
            }

            return output;
        }

        private static string Dump(Transform tf, ref int level, Transform childTf = null)
        {
            var parentOutput = "***";
            if (tf.parent != null) parentOutput = Dump(tf.parent, ref level, tf);

            var go = tf.gameObject;
            var rc = go.GetComponent<RectTransform>();

            var components = tf.GetComponents<Component>();
            var gameBehaviours = components.Where(comp => comp is MonoBehaviour && comp.GetType().Namespace == null).ToArray();
            var gameBehavioursOutput = string.Join(",", gameBehaviours.Select(comp => comp.GetType().Name));
            var builtinBehaviours = components.Where(comp => comp is MonoBehaviour && comp.GetType().Namespace != null).ToArray();
            var builtinBehavioursOutput = string.Join(",", builtinBehaviours.Select(comp => comp.GetType().Name));
            var otherComponents = components.Where(comp => !(comp is MonoBehaviour)).ToArray();
            var otherComponentsOutput = string.Join(",", otherComponents.Select(comp => comp.GetType().Name));

            // FIXME: use format helper for vec3
            var output = $"\n+ {tf.GetSiblingIndex()}:{tf.name}({rc?.sizeDelta.x},{rc?.sizeDelta.y})@({rc?.position.x:0.0},{rc?.position.y:0.0},{rc?.position.z:0.0})/{go.layer}";
            if (gameBehaviours.Length > 0) output += $" game=[{gameBehavioursOutput}]";
            if (builtinBehaviours.Length > 0) output += $" builtin=[{builtinBehavioursOutput}]";
            if (otherComponents.Length > 0) output += $" components=[{otherComponentsOutput}]";
            for (var i = 0; i < tf.childCount; i++)
            {
                var child = tf.GetChild(i);
                output += child == childTf ? "***" : $"\n  - {i}:{child.name}";
            }
            output = output.Replace("\n", $"\n{string.Concat(Enumerable.Repeat("  ", level))}");

            level++;
            return parentOutput.Replace("***", output);
        }

        protected void Update()
        {
            // DEBUG
            if (Input.GetKeyDown("delete"))
            {
                Debug();
            }

            if (Input.GetKeyDown("-"))
            {
                opacity--;
                if (opacity < 0) opacity = 0;
            }

            if (Input.GetKeyDown("="))
            {
                opacity++;
                if (opacity > 10) opacity = 10;
            }

            if (Input.GetKeyDown("backspace"))
            {
                opacity = opacity > 5 ? 0 : 10;
            }

            //debugText = $"{frame.Created}";
            if (Input.GetKeyDown("f10"))
            {
                // FIXME: use utility
                var parent = GameObject.Find("/UIRoot").transform;

                if (overlay.Created)
                    overlay.Destroy();
                else
                {
                    overlay.SetParent(parent);
                }
            }
        }

        internal static void Load()
        {
            if (GameObjectInstance == null)
            {
                GameObjectInstance = new GameObject("ControllerGameObject", typeof(Controller));
                DontDestroyOnLoad(GameObjectInstance);
            }
        }
    }
}
