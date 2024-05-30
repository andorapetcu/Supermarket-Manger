using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tema3MVVM.Helpers;

namespace Tema3MVVM.ViewModels
{
    public class CasierViewModel : INotifyPropertyChanged
    {
        private supermarketEntities _context;
        private string _selectedTable;
        private ObservableCollection<object> _tableData;
        private string _searchText;
        private ObservableCollection<ReceiptItem> _currentReceipt;
        private bool _isReceiptFinalized;
        private int _casierId;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public CasierViewModel(int casierId)
        {
            _casierId = casierId;
            _context = new supermarketEntities();
            SearchByNameCommand = new RelayCommand(param => SearchByName());
            SearchByBarcodeCommand = new RelayCommand(param => SearchByBarcode());
            SearchByExpirationCommand = new RelayCommand(param => SearchByExpiration());
            SearchByProducerCommand = new RelayCommand(param => SearchByProducer());
            SearchByCategoryCommand = new RelayCommand(param => SearchByCategory());
            ReloadTableDataCommand = new RelayCommand(param => LoadTableData());
            AddToReceiptCommand = new RelayCommand4(AddToReceipt, CanAddToReceipt);
            FinalizeReceiptCommand = new RelayCommand4(FinalizeReceipt, CanFinalizeReceipt);
            TableNames = new ObservableCollection<string>
            {
                "Bon",
                "Categorie",
                "Producator",
                "Produs",
                "ProduseVandute",
                "Stoc",
                "Utilizator"
            };
            _currentReceipt = new ObservableCollection<ReceiptItem>();
            SelectedTable = "Produs";
            SearchText = "";
            LoadTableData();
        }

        public ObservableCollection<string> TableNames { get; }
        public ObservableCollection<object> TableData
        {
            get => _tableData;
            set
            {
                _tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public ObservableCollection<ReceiptItem> CurrentReceipt
        {
            get => _currentReceipt;
            private set
            {
                _currentReceipt = value;
                OnPropertyChanged(nameof(CurrentReceipt));
                OnPropertyChanged(nameof(ReceiptTotal));
            }
        }

        public string ReceiptTotal => $"Total: {CurrentReceipt.Sum(item => item.TotalPrice)} lei";

        public bool IsReceiptFinalized
        {
            get => _isReceiptFinalized;
            private set
            {
                _isReceiptFinalized = value;
                OnPropertyChanged(nameof(IsReceiptFinalized));
                ((RelayCommand4)AddToReceiptCommand).RaiseCanExecuteChanged();
                ((RelayCommand4)FinalizeReceiptCommand).RaiseCanExecuteChanged();
            }
        }

        private object _selectedTableRow;
        public object SelectedTableRow
        {
            get { return _selectedTableRow; }
            set
            {
                _selectedTableRow = value;
                OnPropertyChanged(nameof(SelectedTableRow));
            }
        }

        public string SelectedTable
        {
            get => _selectedTable;
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        public ICommand LoadTableDataCommand { get; }
        public ICommand SearchByNameCommand { get; }
        public ICommand SearchByBarcodeCommand { get; }
        public ICommand SearchByExpirationCommand { get; }
        public ICommand SearchByProducerCommand { get; }
        public ICommand SearchByCategoryCommand { get; }
        public ICommand ReloadTableDataCommand { get; }
        public ICommand AddToReceiptCommand { get; }
        public ICommand FinalizeReceiptCommand { get; }

        private void LoadTableData()
        {
            if (string.IsNullOrEmpty(SelectedTable))
                return;

            switch (SelectedTable)
            {
                case "Bon":
                    TableData = new ObservableCollection<object>(_context.Bon.ToList());
                    break;
                case "Categorie":
                    TableData = new ObservableCollection<object>(_context.Categorie.ToList());
                    break;
                case "Producator":
                    TableData = new ObservableCollection<object>(_context.Producator.ToList());
                    break;
                case "Produs":
                    var produse = _context.Produs.Include(p => p.Categorie).Include(p => p.Producator).ToList();
                    TableData = new ObservableCollection<object>(produse);
                    break;
                case "ProduseVandute":
                    TableData = new ObservableCollection<object>(_context.ProduseVandute.ToList());
                    break;
                case "Stoc":
                    TableData = new ObservableCollection<object>(_context.Stoc.ToList());
                    break;
                case "Utilizator":
                    TableData = new ObservableCollection<object>(_context.Utilizator.ToList());
                    break;
            }
        }

        private void SearchByName()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.nume_produs.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByBarcode()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.cod_bare.ToString().Contains(SearchText)
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByExpiration()
        {
            if (DateTime.TryParse(SearchText, out DateTime searchDate))
            {
                var filteredProdus = _context.Stoc
                    .Where(s => DbFunctions.TruncateTime(s.data_expirare) == DbFunctions.TruncateTime(searchDate))
                    .Select(s => s.Produs)
                    .Distinct()
                    .ToList();

                TableData = new ObservableCollection<object>(filteredProdus);
            }
            else
            {
                MessageBox.Show("Invalid date format. Please enter a valid date.");
            }
        }

        private void SearchByProducer()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.Producator.nume_producator.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void SearchByCategory()
        {
            var filteredProdus = _context.Produs.Where(p =>
                p.Categorie.nume_categorie.ToLower().Contains(SearchText.ToLower())
            ).ToList();
            TableData = new ObservableCollection<object>(filteredProdus);
        }

        private void AddToReceipt()
        {
            if (SelectedTableRow is Produs selectedProduct)
            {
                var inputDialog = new InputDialog("Enter quantity:");
                if (inputDialog.ShowDialog() == true)
                {
                    if (int.TryParse(inputDialog.Answer, out int quantity) && quantity > 0)
                    {
                        var activeStocs = _context.Stoc.Where(s => s.IDprodus == selectedProduct.IDprodus && s.exista).OrderBy(s => s.data_aprovizionare).ToList();
                        if (activeStocs.Count == 0)
                        {
                            MessageBox.Show("No active stock found for the selected product.");
                            return;
                        }

                        var correspondingStoc = activeStocs.FirstOrDefault(s => s.cantitate >= quantity);
                        if (correspondingStoc == null)
                        {
                            MessageBox.Show("Insufficient stock for the selected quantity.");
                            return;
                        }

                        if (correspondingStoc.data_expirare <= DateTime.Today)
                        {
                            MessageBox.Show("Selected stock is expired. Cannot add it to the receipt.");
                            correspondingStoc.exista = false; 
                            return;
                        }

                        var existingItem = CurrentReceipt.FirstOrDefault(item => item.Product.IDprodus == selectedProduct.IDprodus);
                        if (existingItem != null)
                        {
                            existingItem.Quantity += quantity;
                        }
                        else
                        {
                            CurrentReceipt.Add(new ReceiptItem { Product = selectedProduct, Stoc = correspondingStoc, Quantity = quantity });
                        }

                        correspondingStoc.cantitate -= quantity;

                        if (correspondingStoc.cantitate == 0)
                        {
                            correspondingStoc.exista = false;
                        }

                        OnPropertyChanged(nameof(CurrentReceipt));
                        OnPropertyChanged(nameof(ReceiptTotal));

                        
                        var lastBon = _context.Bon.OrderByDescending(b => b.IDbon).FirstOrDefault();
                        if (lastBon != null)
                        {
                            _context.InsertProdusVandut(lastBon.IDbon, selectedProduct.IDprodus, quantity, (float)quantity * (float)correspondingStoc.pret_vanzare, true);
                            _context.SaveChanges();
                        }
                        
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid quantity.");
                    }
                }
            }
        }


        private bool CanAddToReceipt() => !IsReceiptFinalized && SelectedTableRow is Produs;

        private void FinalizeReceipt()
        {
            try
            {
                var sumaIncasata = (float)CurrentReceipt.Sum(item => item.TotalPrice);

                _context.InsertBon(DateTime.Now, _casierId, sumaIncasata, true);

                var lastBon = _context.Bon.OrderByDescending(b => b.IDbon).FirstOrDefault();
                if (lastBon != null)
                {
                    foreach (var item in CurrentReceipt)
                    {
                        _context.InsertProdusVandut(lastBon.IDbon, item.Product.IDprodus, item.Quantity, (float)item.TotalPrice, true);
                    }
                    _context.SaveChanges();
                }

                IsReceiptFinalized = true;
                ((RelayCommand4)AddToReceiptCommand).RaiseCanExecuteChanged();
                ((RelayCommand4)FinalizeReceiptCommand).RaiseCanExecuteChanged();

                MessageBox.Show("Receipt finalized and saved to database.");
                ResetReceipt();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while finalizing the receipt: {ex.Message}");
            }
        }

        private void ResetReceipt()
        {
            CurrentReceipt.Clear();
            IsReceiptFinalized = false;
            OnPropertyChanged(nameof(CurrentReceipt));
            OnPropertyChanged(nameof(ReceiptTotal));
        }


        private bool CanFinalizeReceipt() => CurrentReceipt.Any() && !IsReceiptFinalized;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ReceiptItem
    {
        public Produs Product { get; set; }
        public Stoc Stoc { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => (decimal)Stoc.pret_vanzare * Quantity;
    }
}
