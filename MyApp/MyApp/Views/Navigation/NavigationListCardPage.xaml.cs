using MyApp.DataService;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using MyApp.Views.Detail;
using MyApp.Views.ButtonNavigation;
using System;

namespace MyApp.Views.Navigation
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationListCardPage
    {
        public BanknotesRecognitionPage NewBanknotesRecognitionPage { get; private set;}
        public int ItemId { get; private set; }
        public NavigationListCardPage()
        {
            InitializeComponent();
            this.BindingContext = NavigationDataService.Instance.NavigationViewModel;
         
        }
        private async void BanknotesItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            // удалить боттом навигэйшн
            NewBanknotesRecognitionPage = new BanknotesRecognitionPage();
            await Navigation.PushAsync(NewBanknotesRecognitionPage);
        }
        private async void PrintedTextItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {         
            await Navigation.PushAsync(new RecognitionPrintedPage());          
        }
        private async void HandwrittenTextItem_Clicked(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new RecognitionHandwrittenPage());
        }
    }
}
