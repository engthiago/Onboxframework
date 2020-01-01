using Onbox.Mvvm.V1;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Onbox.Sandbox.Revit.Commands.Inher;

namespace Onbox.Sandbox.Revit.Commands
{
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : ViewWindowBase, IOrderView
    {
        public bool IsEditing { get; set; }

        public List<User> Users { get; set; }
        private readonly IServerService serverService;
        private readonly IMessageService messageService;

        public OrderView(IServerService serverService, IMessageService messageService)
        {
            this.InitializeComponent();
            this.Title = "MVC test";
            this.serverService = serverService;
            this.messageService = messageService;
        }

        public async override void OnInit()
        {
            await this.PerformAsync(async () =>
            {
                this.Users = await this.serverService.GetUsersAsync();
            },
            (error) =>
            {
                this.messageService.Warning(error);
                this.Users = new List<User>();
            });
        }

        private async void Confirm(object sender, RoutedEventArgs e)
        {
            if (await this.Save())
            {
                this.DialogResult = true;
            }
        }

        private async Task<bool> Save()
        {
            return await this.PerformAsync(async () =>
            {
                await this.serverService.SaveUsersAsync(this.Users);
            },
            (error) =>
            {
                this.messageService.Warning(error);
            });
        }

        public override bool CanCloseDialog()
        {
            if (this.DialogResult == true)
            {
                return true;
            }

            if (this.IsEditing)
            {
                MessageBox.Show("Please Confirm editing first");
                return false;
            }

            return true;
        }



        private async void Save(object sender, RoutedEventArgs e)
        {
            await this.Save();
        }

        private void BeginEditing(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            this.IsEditing = true;
        }

        private void EndEditing(object sender, DataGridCellEditEndingEventArgs e)
        {
            this.IsEditing = false;
        }
    }
}
