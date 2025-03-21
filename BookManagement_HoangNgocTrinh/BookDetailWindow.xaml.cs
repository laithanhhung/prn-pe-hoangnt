﻿using BookManagement.BLL.services;
using BookManagement.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookManagement_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for BookDetailWindow.xaml
    /// </summary>
    public partial class BookDetailWindow : Window
    {
        //book service: để save
        private BookService _bookService = new BookService();

        //category service: để lấy ra category làm dropdown (ComboBox)
        private CategoryService _categoryService = new CategoryService();

        //flag: phân biệt create/update
        public Book? EditedOne { get; set; } = null;

        //nếu Create
        //màn hình trắng chờ fill từng prop

        //nếu Edit
        //load dữ liệu từ EditedOne vào các textbox, combobox, datepicker

        //Dù là create hay edit thì cũng cần lấy ra toàn bộ category để fill vào combobox
        //Nhưng mode edit thì phải jump vào category của EditedOne để fill vào combobox

        public BookDetailWindow()
        {
            InitializeComponent();
        }

        private void FillElement()
        {
            BookIdTextBox.Text = EditedOne.BookId.ToString();
            BookNameTextBox.Text = EditedOne.BookName;
            DescriptionTextBox.Text = EditedOne.Description;
            PublicationDateDatePicker.SelectedDate = EditedOne.PublicationDate;
            QuantityTextBox.Text = EditedOne.Quantity.ToString();
            PriceTextBox.Text = EditedOne.Price.ToString();
            AuthorTextBox.Text = EditedOne.Author;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //B1: fill combobox
            List<string> categoryNames = this._categoryService.GetALlBookCategory().ToList().Select(x => x.BookGenreType).ToList();
            foreach (string categoryName in categoryNames)
            {
                BookCategoryIdComboBox.Items.Add(categoryName);
            }
            //B2: fill element khi mode edit
            if (EditedOne != null)
            {
                FillElement();
                //B3: fill combobox theo category của EditedOne
                BookCategory Category = this._categoryService.GetALlBookCategory().ToList().FirstOrDefault(x => x.BookCategoryId == EditedOne.BookCategoryId);
                if (Category != null)
                {
                    BookCategoryIdComboBox.SelectedItem = Category.BookGenreType;
                }
                //B4: Phế võ công nhập ID
                BookIdTextBox.IsEnabled = false;
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //reset EditedOne
            this.EditedOne = null;
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckValidInput()) return;
            Book book = new Book();
            book.BookId = int.Parse(BookIdTextBox.Text);
            book.BookName = BookNameTextBox.Text;
            book.Description = DescriptionTextBox.Text;
            book.PublicationDate = PublicationDateDatePicker.SelectedDate.Value;
            book.Quantity = int.Parse(QuantityTextBox.Text);
            book.Price = double.Parse(PriceTextBox.Text);
            book.Author = AuthorTextBox.Text;
            BookCategory bookCategory = this._categoryService.GetALlBookCategory().ToList().FirstOrDefault(x => x.BookGenreType == BookCategoryIdComboBox.SelectedItem.ToString());



            if (bookCategory != null)
            {
                book.BookCategoryId = bookCategory.BookCategoryId;
            }

            //nếu là edit thì gán lại vào EditedOne
            if (EditedOne != null)
            {
                EditedOne = book;
                _bookService.UpdateBook(EditedOne);
            }
            //nếu là create thì thêm vào list
            else
            {
                _bookService.AddNewBook(book);
            }
            EditedOne = null;
            this.Close();
        }

        private Boolean CheckValidInput()
        {
            if (string.IsNullOrWhiteSpace(BookIdTextBox.Text.Trim())){
                MessageBox.Show("Id must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(BookIdTextBox.Text, out int bookId) || bookId < 0)
            {
                MessageBox.Show("Id must be a number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(BookNameTextBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(PublicationDateDatePicker.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(QuantityTextBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }


            if (!int.TryParse(QuantityTextBox.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity must be a positive integer!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(PriceTextBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0)
            {
                MessageBox.Show("Price must be a positive number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(AuthorTextBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(BookCategoryIdComboBox.Text.Trim()))
            {
                MessageBox.Show("Book name must be filled!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;

            //if (string.IsNullOrWhiteSpace(BookIdTextBox.Text.Trim()) || string.IsNullOrWhiteSpace(BookNameTextBox.Text.Trim()) || string.IsNullOrWhiteSpace(DescriptionTextBox.Text.Trim()) || PublicationDateDatePicker.SelectedDate == null || string.IsNullOrWhiteSpace(QuantityTextBox.Text.Trim()) || string.IsNullOrWhiteSpace(PriceTextBox.Text.Trim()) || string.IsNullOrWhiteSpace(AuthorTextBox.Text.Trim()) || BookCategoryIdComboBox.SelectedItem == null)
            //{
            //    MessageBox.Show("Please fill all the fields!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return false;
            //}
        }
    }
}
