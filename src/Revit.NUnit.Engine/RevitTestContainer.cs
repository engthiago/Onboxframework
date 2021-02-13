//using Autodesk.Revit.ApplicationServices;
//using Autodesk.Revit.DB;
//using System;

//namespace Onbox.Revit.NUnit
//{
//    public static class RevitTestContainer
//    {
//        private static Document revitDoc;
//        private static Application revitApp;
//        private static bool isInitialized;

//        public static void Initialize(Document doc, Application app)
//        {
//            isInitialized = true;
//            revitDoc = doc;
//            revitApp = app;
//        }

//        public static Document GetRevitDocument()
//        {
//            CheckInitiazed();
//            return revitDoc;
//        }

//        public static Application GetRevitApplication()
//        {
//            CheckInitiazed();
//            return revitApp;
//        }

//        private static void CheckInitiazed()
//        {
//            if (!isInitialized)
//            {
//                //throw new Exception("Revit Container has not been initialized!");
//            }
//        }
//    }
//}