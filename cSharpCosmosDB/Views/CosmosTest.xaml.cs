using cSharpCosmosDB.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace cSharpCosmosDB.Views
{
    public partial class CosmosTest : ContentPage
    {
        public CosmosTest()
        {
            InitializeComponent();
        }

        async void btnConnect_Click(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                //var connected = await cosmos.ConnectTest();
                await cosmos.StackOverflow();
                lblConnected.Text = "Connected: true";
            }
            catch (Exception ex)
            {
                lblConnected.Text = "An error occurred: " + ex.Message;
                Debug.WriteLine(ex.Message);
            }
        }

        async void btnGenerate_Click(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                var meeting = await cosmos.GenerateMeeting();
                lblCode.Text = "CODE: " + meeting.Code;
            }
            catch (Exception ex)
            {
                lblConnected.Text = "An error occurred: " + ex.Message;
            }
        }

        async void btnCountBooks_Click(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                string id = await cosmos.SaveBook();
                long count = await cosmos.BookCount();
                lblCountBooks.Text = "Book Count Count: " + count;
            }
            catch (Exception ex)
            {
                lblCountBooks.Text = "An error occurred: " + ex.Message;
            }
        }

        void btnCount_Click(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                Task<long> task = cosmos.MeetingCount();
                task.Wait();
                long count = task.Result;
                lblCount.Text = "Meeting Count: " + count;
            }
            catch (Exception ex)
            {
                lblCount.Text = "An error occurred: " + ex.Message;
            }
        }

        async void btnCount_Click2(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                var count = await cosmos.MeetingCount();
                lblCount.Text = "Meeting Count: " + count;
            }
            catch (Exception ex)
            {
                lblCount.Text = "An error occurred: " + ex.Message;
            }
        }

        async void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                var cosmos = new CosmosDataStore();
                await cosmos.DeleteAllMeetings();
                lblCount.Text = "All meetings deleted";
            }
            catch (Exception ex)
            {
                lblCount.Text = "An error occurred: " + ex.Message;
            }
        }
    }
}
