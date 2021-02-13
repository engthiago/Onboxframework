using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using System;

namespace Onbox.Revit.NUnit
{
    public static class RevitRemoteContainer
    {
        private static Document revitDoc;
        private static Application revitApp;
        private static string filePath;
        private static bool isInitialized;

        public static void Initialize(Document doc, Application app, string path)
        {
            isInitialized = true;
            revitDoc = doc;
            revitApp = app;
            filePath = path;
        }

        public static Document GetRevitDocument()
        {
            CheckInitiazed();
            return revitDoc;
        }

        public static Application GetRevitApplication()
        {
            CheckInitiazed();
            return revitApp;
        }
        public static string GetFilePath()
        {
            CheckInitiazed();
            return filePath;
        }

        private static void CheckInitiazed()
        {
            if (!isInitialized)
            {
                //throw new Exception("Revit Container has not been initialized!");
            }
        }
    }
}