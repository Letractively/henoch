using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using AdventureWorksSL.ServiceReference1;

namespace AdventureWorksSL
{
    public partial class MainPage : UserControl
    {
        protected ProductServiceSLClient productService = new ProductServiceSLClient();

        public MainPage()
        {
            InitializeComponent();

            productService.GetProductCategoriesCompleted += productService_GetProductCategoriesCompleted;
            productService.GetProductsByCategoryCompleted += productService_GetProductsByCategoryCompleted;
            productService.GetProductCategoriesAsync();
        }

        private void CategoriesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductCategory category = CategoriesDataGrid.SelectedItem as ProductCategory;
            productService.GetProductsByCategoryAsync(category.ProductCategoryID);
        }

        void productService_GetProductsByCategoryCompleted(object sender, GetProductsByCategoryCompletedEventArgs e)
        {
            ProductsDataGrid.ItemsSource = e.Result;
        }

        void productService_GetProductCategoriesCompleted(object sender, GetProductCategoriesCompletedEventArgs e)
        {
            CategoriesDataGrid.ItemsSource = e.Result;
        }

    }
}
