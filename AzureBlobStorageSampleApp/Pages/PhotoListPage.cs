using System;
using System.Linq;
using AzureBlobStorageSampleApp.Mobile.Shared;
using AzureBlobStorageSampleApp.Shared;
using Xamarin.Forms;

namespace AzureBlobStorageSampleApp
{
    public class PhotoListPage : BaseContentPage<PhotoListViewModel>
    {
        readonly RefreshView _photosCollectionRefreshView;

        public PhotoListPage()
        {
            var addPhotosButton = new ToolbarItem
            {
                Text = "+",
                AutomationId = AutomationIdConstants.AddPhotoButton
            };
            addPhotosButton.Clicked += HandleAddContactButtonClicked;

            ToolbarItems.Add(addPhotosButton);

            var photoCollectionView = new CollectionView
            {
                ItemTemplate = new PhotoDataTemplate(),
                SelectionMode = SelectionMode.Single,
                AutomationId = AutomationIdConstants.PhotosCollectionView,
            };
            photoCollectionView.SelectionChanged += HandlePhotoCollectionSelectionChanged;
            photoCollectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(PhotoListViewModel.AllPhotosList));

            _photosCollectionRefreshView = new RefreshView
            {
                Content = photoCollectionView
            };
            _photosCollectionRefreshView.SetBinding(RefreshView.IsRefreshingProperty, nameof(PhotoListViewModel.IsRefreshing));
            _photosCollectionRefreshView.SetBinding(RefreshView.CommandProperty, nameof(PhotoListViewModel.RefreshCommand));

            Title = PageTitles.PhotoListPage;

            Content = _photosCollectionRefreshView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _photosCollectionRefreshView.IsRefreshing = true;
        }

        async void HandlePhotoCollectionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = (CollectionView)sender;
            collectionView.SelectedItem = null;

            if (e.CurrentSelection.FirstOrDefault() is PhotoModel selectedPhoto)
            {
                await Navigation.PushAsync(new PhotoDetailsPage(selectedPhoto));
            }
        }

        void HandleAddContactButtonClicked(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await Navigation.PushModalAsync(new BaseNavigationPage(new AddPhotoPage())));
    }
}
