﻿using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

namespace Onbox.Revit.NUnit.Core.Internal
{
    public static class RemoteContainer
    {
        internal static Application revitApp;
        internal static string addinDirectory;
        internal static string workDirectory;
        internal static bool isInitialized;
        internal static string workItemId;

        public static void Register(Application app, string addinsDir, string workDir, string workId)
        {
            isInitialized = true;
            revitApp = app;
            addinDirectory = addinsDir;
            workDirectory = workDir;
            workItemId = workId;
        }

        public static Application GetRevitApplication()
        {
            CheckInitiazed();
            return revitApp;
        }

        public static string GetWorkDirectory()
        {
            CheckInitiazed();
            return workDirectory;
        }

        public static string GetAddinDirectory()
        {
            CheckInitiazed();
            return addinDirectory;
        }

        public static string GetWorkItemId()
        {
            CheckInitiazed();
            return workItemId;
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