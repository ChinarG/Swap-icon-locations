using System;
using UnityEditor;
using UnityEngine;



namespace ChinarX.Window
{
    public class ChinarWelcomeWindow : EditorWindow
    {
        private          string              version          = "Version : 1.0.0";
        private readonly Rect                versionRect      = new Rect(5f,   630f, 125f, 20f);
        private readonly Rect                welcomeIntroRect = new Rect(120f, 15f,  666f, 40f);
        private static   ChinarWelcomeWindow _thisWindow;
        private static   Item                _chinarSiteTexture = new Item(new Rect(66f,  66f,  266f, 266f), _chinarSiteTexture.Texture, null);
        private static   Item                _weChatOr          = new Item(new Rect(332f, 66f,  266f, 266f), _weChatOr.Texture,          "111111111");
        private static   Item                _qqGroupTexture    = new Item(new Rect(222f, 366f, 66f,  50f),  _qqGroupTexture.Texture,    null);
        private          Item                qqGroupTitle       = new Item(new Rect(288f, 376f, 250f, 20f),  null,                       " 加入技术支持社群");
        private          Item                qqGroupContent     = new Item(new Rect(288f, 396f, 250f, 30f),  null,                       "点击此处，即可加入 —— QQ群:806091680");
        private Item chinarSiteContent = new Item(new Rect(250f, 350f, 250f, 20f), null, " 加入技术支持社群");


        private struct Item
        {
            public          Rect    Rect;
            public          Texture Texture;
            public readonly string  Content;


            public Item(Rect rect, Texture texture, string content)
            {
                Rect    = rect;
                Texture = texture;
                Content = content;
            }
        }


        public void OnEnable()
        {
            _chinarSiteTexture.Texture = LoadTexture("ChinarSiteIcon.png");
            _weChatOr.Texture          = LoadTexture("Wechat.png");
            _qqGroupTexture.Texture    = LoadTexture("QQ.png");
        }


        static Texture LoadTexture(string textureName)
        {
            return (Texture) AssetDatabase.LoadAssetAtPath("Assets/Chinar/Editor/Editor Default Resources/" + textureName, typeof(Texture));
        }


        [MenuItem("Chinar/欢迎界面", false, 1)]
        public static void ShowChinarWelcomeWindow()
        {
            _thisWindow         = GetWindow<ChinarWelcomeWindow>(true, "关注 & 支持 Chinar");
            _thisWindow.minSize = _thisWindow.maxSize = new Vector2(666f, 666f);
            _thisWindow.Show();
        }


        private void OnGUI()
        {
            GUI.Label(welcomeIntroRect,               "Chinar 的初衷是将一种简单快捷的生活方式带给世人,使有限时间，具备无限可能！");
            GUI.Label(new Rect(160f, 35f, 666f, 40f), "关注微信公众号 & 加入社区群 —— Chinar 将为您提供更优质内容");
            GUI.Label(versionRect,                    version);
            GUI.DrawTexture(_weChatOr.Rect, _weChatOr.Texture);
            Link(_qqGroupTexture.Rect,    _qqGroupTexture.Texture,    null, false);
            Link(_chinarSiteTexture.Rect, _chinarSiteTexture.Texture, null, false);
            Link(qqGroupTitle.Rect,       null,                       qqGroupTitle.Content);
            Link(qqGroupContent.Rect,     null,                       qqGroupContent.Content);
            Link(chinarSiteContent.Rect,     null, chinarSiteContent.Content);
            
            if (Event.current.type == EventType.MouseUp)
            {
                Vector2 mousePosition = Event.current.mousePosition;
                if (_qqGroupTexture.Rect.Contains(mousePosition) || qqGroupTitle.Rect.Contains(mousePosition) || qqGroupContent.Rect.Contains(mousePosition))
                {
                    Application.OpenURL("http://shang.qq.com/wpa/qunwpa?idkey=5dacafe26abe29923ed6a5d8cf76248b5b68f0fcdc599fcd231007814eb75c4d");
                }
                else if (_chinarSiteTexture.Rect.Contains(mousePosition))
                {
                    Application.OpenURL("http://www.chinar.xin");
                }
            }
        }


        private void Link(Rect rect, Texture texture, string content, bool isLabel = true)
        {
            if (isLabel)
            {
                GUI.Label(rect, content);
            }
            else
            {
                GUI.DrawTexture(rect, texture);
            }

            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
        }
    }


    public class ChinarInitializeOnLoadWindow : EditorWindow
    {
        private static ChinarInitializeOnLoadWindow _thisWindow;
        private static bool                         _isShow  = true;
        private        float                        timeSpan = 0;


        [InitializeOnLoadMethod]
        private static void InitializeOnLoadWindow()
        {
            if (PlayerPrefs.GetString("Chinar_Import_DateTime") == String.Empty)
            {
                PlayerPrefs.SetString("Chinar_Import_DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                if ((DateTime.Now - DateTime.ParseExact(PlayerPrefs.GetString("Chinar_Import_DateTime"), "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)).Days >= 7)
                {
                    PlayerPrefs.SetString("Chinar_Import_DateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    _isShow = false;
                    return;
                }
            }

            _thisWindow         = GetWindow<ChinarInitializeOnLoadWindow>(false, "Chinar InitPanel");
            _thisWindow.minSize = _thisWindow.maxSize = Vector2.zero;
        }


        private void Update()
        {
            timeSpan += Time.deltaTime;
            if (!(timeSpan > 0.5f) || !_isShow) return;
            ChinarWelcomeWindow.ShowChinarWelcomeWindow();
            _thisWindow.Close();
        }
    }
}