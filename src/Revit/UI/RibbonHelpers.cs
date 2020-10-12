using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Onbox.Revit.VDev.UI
{
    public class RibbonHelpers
    {
        private readonly ImageManager imageManager;

        public RibbonHelpers(ImageManager imageManager)
        {
            this.imageManager = imageManager;
        }

        public PushButton CreatePushButton(RibbonPanel targetPanel, string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescrip = "")
        {
            var currentDll = commandType.Assembly.Location;
            string fullname = commandType.FullName;

            PushButton currentBtn = targetPanel.AddItem(new PushButtonData(targetName, targetName, currentDll, fullname)) as PushButton;
            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage32 = this.imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                currentBtn.LargeImage = currentImage32;
            }
            catch (Exception) { }

            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage16 = this.imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                currentBtn.Image = currentImage16;
            }
            catch (Exception) { }

            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescrip;

            return currentBtn;
        }

        public ToggleButton CreateToggleButton(RibbonPanel targetPanel, string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescrip = "")
        {
            var currentDll = commandType.Assembly.Location;
            string fullname = commandType.FullName;

            ToggleButton currentBtn = targetPanel.AddItem(new ToggleButtonData(targetName, targetName, currentDll, fullname)) as ToggleButton;
            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage32 = this.imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                currentBtn.LargeImage = currentImage32;
            }
            catch (Exception) { }

            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage16 = this.imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                currentBtn.Image = currentImage16;
            }
            catch (Exception) { }

            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescrip;

            return currentBtn;
        }

        public PushButtonData CreatePushButtonData(string targetName, string targetImage, Type commandType, string targetToolTip = "", string targetDescrip = "")
        {
            var currentDll = commandType.Assembly.Location;
            string fullname = commandType.FullName;

            PushButtonData currentBtn = new PushButtonData(targetName, targetName, currentDll, fullname);
            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage32 = this.imageManager.ConvertBitmapSource(targetImage + "32.png", commandType.Assembly);
                currentBtn.LargeImage = currentImage32;
            }
            catch (Exception) { }

            try
            {
                System.Windows.Media.Imaging.BitmapImage currentImage16 = this.imageManager.ConvertBitmapSource(targetImage + "16.png", commandType.Assembly);
                currentBtn.Image = currentImage16;
            }
            catch (Exception) { }

            currentBtn.ToolTip = targetToolTip;
            currentBtn.LongDescription = targetDescrip;

            return currentBtn;
        }

        public SplitButton CreateSplitButton(RibbonPanel targetPanel, IList<PushButtonData> targetPushButtons)
        {
            SplitButton currentSplitButton = null;
            if (targetPushButtons.Count > 0)
            {
                string targetName = targetPushButtons.FirstOrDefault().Name;
                currentSplitButton = targetPanel.AddItem(new SplitButtonData(targetName, targetName)) as SplitButton;
                foreach (PushButtonData currentPushButton in targetPushButtons)
                {
                    currentSplitButton.AddPushButton(currentPushButton);
                }
            }

            return currentSplitButton;
        }

    }
}