using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Core.Objects.DataClasses;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Data.Entity;
using Tema3MVVM.Helpers;
using System.Collections.Generic;

namespace Tema3MVVM.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private supermarketEntities _context;
        private string _selectedTable;
        private ObservableCollection<object> _tableData;

        private string _numeCategorie;

        private string _numeProducator;
        private string _taraOrigine;

        private string _numeProdus;
        private long _codBare;
        private int _idCategorie;
        private long _idProducator;

        private long _idProdus;
        private long _cantitate;
        private string _unitateDeMasura;
        private DateTime _dataAprovizionare;
        private DateTime _dataExpirare;
        private float _pretAchizitie;
        private float _pretVanzare;

        private string _numeUtilizator;
        private string _parola;
        private string _tipUtilizator;

        public ICommand DeleteSelectedRowCommand { get; }
        public ICommand DisplayLargestReceiptCommand { get; }
        public AdminViewModel()
        {
            _context = new supermarketEntities();
            TableNames = new ObservableCollection<string>
            {
                "Bon",
                "Categorie",
                "Producator",
                "Produs",
                "ProduseVandute",
                "Stoc",
                "Utilizator",
                "TotalCategorie"
            };
            LoadTableDataCommand = new RelayCommand4(LoadTableData);
            AddCategorieCommand = new RelayCommand4(AddCategorie);
            AddProducatorCommand = new RelayCommand4(AddProducator);
            AddProdusCommand = new RelayCommand4(AddProdus);
            AddStocCommand = new RelayCommand4(AddStoc);
            AddUtilizatorCommand = new RelayCommand4(AddUtilizator);

            UpdateBonCommand = new RelayCommand(param => UpdateBonMethod(param));
            UpdateCategorieCommand = new RelayCommand(param => UpdateCategorieMethod(param));
            UpdateProducatorCommand = new RelayCommand(param => UpdateProducatorMethod(param));
            UpdateProdusCommand = new RelayCommand(param => UpdateProdusMethod(param));
            UpdateProdusVandutCommand = new RelayCommand(param => UpdateProdusVandutMethod(param));
            UpdateStocCommand = new RelayCommand(param => UpdateStocMethod(param));
            UpdateUtilizatorCommand = new RelayCommand(param => UpdateUtilizatorMethod(param));
            UpdateSelectedRowCommand = new RelayCommand4(UpdateSelectedRow);
            DeleteSelectedRowCommand = new RelayCommand4(DeleteSelectedRow);
            LoadProducatorProductsCommand = new RelayCommand4(LoadProducatorProducts);
            Producatori = new ObservableCollection<Producator>();
            LoadProducatoriCommand = new RelayCommand4(LoadProducatori);
            DisplayLargestReceiptCommand = new RelayCommand4(DisplayLargestReceipt);
            DisplayUserReceiptsCommand = new RelayCommand2<object>(DisplayUserReceipts);
            LoadProducatori();

        }
        public ICommand LoadProducatoriCommand { get; }
        private void LoadProducatori()
        {
            var producatori = _context.Producator.ToList();
            Producatori.Clear();
            foreach (var producator in producatori)
            {
                Producatori.Add(producator);
            }
        }


        private Dictionary<string, float> _categoryTotals = new Dictionary<string, float>();

        private void DisplayCategoryTotal()
        {
            if (!string.IsNullOrEmpty(NumeCategorie))
            {
                var productsInCategory = _context.Produs
                    .Where(p => p.Categorie.nume_categorie == NumeCategorie)
                    .ToList();

                float total = 0;
                foreach (var product in productsInCategory)
                {
                    total += product.Stoc.FirstOrDefault()?.pret_vanzare * product.Stoc.FirstOrDefault()?.cantitate ?? 0;
                }

                _categoryTotals[NumeCategorie] = total;

                MessageBox.Show($"Suma totală pentru categoria '{NumeCategorie}' este: {total}", "Sumă totală pentru categorie", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Selectați o categorie din combobox.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void DeleteSelectedRow()
        {
            if (SelectedTableRow != null)
            {
                switch (SelectedTable)
                {
                    case "Bon":
                        if (SelectedTableRow is Bon bon)
                        {
                            _context.DeleteBon(bon.IDbon);
                            LoadTableData();
                            MessageBox.Show("Bonul a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Categorie":
                        if (SelectedTableRow is Categorie categorie)
                        {
                            _context.DeleteCategorie(categorie.IDcategorie);
                            LoadTableData();
                            MessageBox.Show("Categoria a fost stearsa!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Producator":
                        if (SelectedTableRow is Producator producator)
                        {
                            _context.DeleteProducator(producator.IDproducator);
                            LoadTableData();
                            MessageBox.Show("Producatorul a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Produs":
                        if (SelectedTableRow is Produs produs)
                        {
                            _context.DeleteProdus(produs.IDprodus);
                            LoadTableData();
                            MessageBox.Show("Produsul a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "ProduseVandute":
                        if (SelectedTableRow is ProduseVandute produseVandute)
                        {
                            _context.DeleteProdusVandut(produseVandute.IDbon, produseVandute.IDprodus);
                            LoadTableData();
                            MessageBox.Show("Produsul Vandut a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Stoc":
                        if (SelectedTableRow is Stoc stoc)
                        {
                            _context.DeleteStoc(stoc.IDstoc);
                            LoadTableData();
                            MessageBox.Show("Stocul a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    case "Utilizator":
                        if (SelectedTableRow is Utilizator utilizator)
                        {
                            _context.DeleteUtilizator(utilizator.IDutilizator);
                            LoadTableData();
                            MessageBox.Show("Utilizatorul a fost sters!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        break;
                    default:
                        break;
                }

            }
            else
                MessageBox.Show("Nu ati selectat rand", "Alegeti din nou", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public string NumeCategorie
        {
            get => _numeCategorie;
            set
            {
                _numeCategorie = value;
                OnPropertyChanged(nameof(NumeCategorie));
            }
        }

        public string NumeProducator
        {
            get => _numeProducator;
            set
            {
                _numeProducator = value;
                OnPropertyChanged(nameof(NumeProducator));
            }
        }

        public string TaraOrigine
        {
            get => _taraOrigine;
            set
            {
                _taraOrigine = value;
                OnPropertyChanged(nameof(TaraOrigine));
            }
        }

        public string NumeProdus
        {
            get => _numeProdus;
            set
            {
                _numeProdus = value;
                OnPropertyChanged(nameof(NumeProdus));
            }
        }

        public long CodBare
        {
            get => _codBare;
            set
            {
                _codBare = value;
                OnPropertyChanged(nameof(CodBare));
            }
        }

        public int IdCategorie
        {
            get => _idCategorie;
            set
            {
                _idCategorie = value;
                OnPropertyChanged(nameof(IdCategorie));
            }
        }

        public long IdProducator
        {
            get => _idProducator;
            set
            {
                _idProducator = value;
                OnPropertyChanged(nameof(IdProducator));
            }
        }

        public long IdProdus
        {
            get => _idProdus;
            set
            {
                _idProdus = value;
                OnPropertyChanged(nameof(IdProdus));
            }
        }

        public long Cantitate
        {
            get => _cantitate;
            set
            {
                _cantitate = value;
                OnPropertyChanged(nameof(Cantitate));
            }
        }

        public string UnitateDeMasura
        {
            get => _unitateDeMasura;
            set
            {
                _unitateDeMasura = value;
                OnPropertyChanged(nameof(UnitateDeMasura));
            }
        }
        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        public DateTime DataAprovizionare
        {
            get => _dataAprovizionare;
            set
            {
                _dataAprovizionare = value;
                OnPropertyChanged(nameof(DataAprovizionare));
            }
        }

        public DateTime DataExpirare
        {
            get => _dataExpirare;
            set
            {
                _dataExpirare = value;
                OnPropertyChanged(nameof(DataExpirare));
            }
        }

        public float PretAchizitie
        {
            get => _pretAchizitie;
            set
            {
                _pretAchizitie = value;
                OnPropertyChanged(nameof(PretAchizitie));
            }
        }

        public float PretVanzare
        {
            get => _pretVanzare;
            set
            {
                _pretVanzare = value;
                OnPropertyChanged(nameof(PretVanzare));
            }
        }

        public string NumeUtilizator
        {
            get => _numeUtilizator;
            set
            {
                _numeUtilizator = value;
                OnPropertyChanged(nameof(NumeUtilizator));
            }
        }

        public string Parola
        {
            get => _parola;
            set
            {
                _parola = value;
                OnPropertyChanged(nameof(Parola));
            }
        }

        public string TipUtilizator
        {
            get => _tipUtilizator;
            set
            {
                _tipUtilizator = value;
                OnPropertyChanged(nameof(TipUtilizator));
            }
        }

        private ObservableCollection<Producator> _producatori;
        public ObservableCollection<Producator> Producatori
        {
            get => _producatori;
            set
            {
                _producatori = value;
                OnPropertyChanged(nameof(Producatori));
            }
        }

        private Producator _selectedProducator;
        public Producator SelectedProducator
        {
            get => _selectedProducator;
            set
            {
                _selectedProducator = value;
                OnPropertyChanged(nameof(SelectedProducator));
            }
        }

        private ObservableCollection<IGrouping<string, Produs>> _producatorProducts;
        public ObservableCollection<IGrouping<string, Produs>> ProducatorProducts
        {
            get => _producatorProducts;
            set
            {
                _producatorProducts = value;
                OnPropertyChanged(nameof(ProducatorProducts));
            }
        }

        private void LoadProducatorProducts()
        {
            if (SelectedProducator != null)
            {
                var products = _context.Produs
                    .Where(p => p.IDproducator == SelectedProducator.IDproducator)
                    .Include(p => p.Categorie)
                    .ToList()
                    .GroupBy(p => p.Categorie.nume_categorie);

                ProducatorProducts = new ObservableCollection<IGrouping<string, Produs>>(products);
            }
            else
            {
                MessageBox.Show("Selectați un producător.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand LoadTableDataCommand { get; }
        public ICommand AddCategorieCommand { get; }
        public ICommand AddProducatorCommand { get; }
        public ICommand AddProdusCommand { get; }
        public ICommand AddStocCommand { get; }
        public ICommand AddUtilizatorCommand { get; }

        public ICommand UpdateBonCommand { get; }
        public ICommand UpdateCategorieCommand { get; }
        public ICommand UpdateProducatorCommand { get; }
        public ICommand UpdateProdusCommand { get; }
        public ICommand UpdateProdusVandutCommand { get; }
        public ICommand UpdateStocCommand { get; }
        public ICommand UpdateUtilizatorCommand { get; }

        public ICommand UpdateSelectedRowCommand { get; }

        public ICommand LoadProducatorProductsCommand { get; }


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
                    var stocuri = _context.Stoc.Include(s => s.Produs).ToList();
                    foreach (var stoc in stocuri)
                    {
                        Console.WriteLine($"ID Produs: {stoc.IDprodus}, Nume Produs: {stoc.Produs?.nume_produs}");
                    }
                    TableData = new ObservableCollection<object>(stocuri);
                    break;
                    ;
                case "Utilizator":
                    TableData = new ObservableCollection<object>(_context.Utilizator.ToList());
                    break;
                case "TotalCategorie":
                    TableData = new ObservableCollection<object>(); 

                    foreach (var category in _context.Categorie.ToList())
                    {
                        var productsInCategory = _context.Produs
                            .Where(p => p.Categorie.nume_categorie == NumeCategorie)
                            .ToList();

                        float total = 0;
                        foreach (var product in productsInCategory)
                        {
                            foreach (var stoc in product.Stoc)
                            {
                                total += stoc.pret_vanzare * stoc.cantitate;
                            }
                        }

                        TableData.Add(new { NumeCategorie = category.nume_categorie, TotalCategorie = total });
                    }
                    break;
            }
        }



        private void AddCategorie()
        {
            if (!string.IsNullOrWhiteSpace(NumeCategorie))
            {
                _context.InsertCategorie(NumeCategorie, true);
                LoadTableData();
                MessageBox.Show("Categoria a fost adăugată cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddProducator()
        {
            if (!string.IsNullOrWhiteSpace(NumeProducator) && !string.IsNullOrWhiteSpace(TaraOrigine))
            {
                _context.InsertProducator(NumeProducator, TaraOrigine, true);
                LoadTableData();
                MessageBox.Show("Producătorul a fost adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddProdus()
        {
            if (!string.IsNullOrWhiteSpace(NumeProdus) && CodBare > 0 && IdCategorie > 0 && IdProducator > 0)
            {
                _context.InsertProdus(NumeProdus, CodBare, IdCategorie, IdProducator, true);
                LoadTableData();
                MessageBox.Show("Produsul a fost adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddStoc()
        {
            float adaosComercial;
            try
            {
                string adaosFilePath = "adaos.txt";
                if (File.Exists(adaosFilePath))
                {
                    string adaosContent = File.ReadAllText(adaosFilePath);
                    Console.WriteLine("Content of adaos.txt: " + adaosContent); 
                    if (float.TryParse(adaosContent, out adaosComercial))
                    {
                        float pretVanzare = PretAchizitie + (PretAchizitie * adaosComercial);

                        _context.InsertStoc(IdProdus, Cantitate, UnitateDeMasura, DataAprovizionare, DataExpirare, PretAchizitie, pretVanzare, true);

                        LoadTableData();
                        MessageBox.Show("Stocul a fost adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                MessageBox.Show("Nu s-a putut citi adaosul comercial din fișierul adaos.txt.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"A apărut o eroare la citirea adaosului comercial: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void AddUtilizator()
        {
            if (!string.IsNullOrWhiteSpace(NumeUtilizator) && !string.IsNullOrWhiteSpace(Parola) && !string.IsNullOrWhiteSpace(TipUtilizator))
            {
                _context.InsertUtilizator(NumeUtilizator, Parola, TipUtilizator, true);
                LoadTableData();
                MessageBox.Show("Utilizatorul a fost adăugat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void UpdateSelectedRow()
        {
            if (SelectedTableRow != null)
            {
                switch (SelectedTable)
                {
                    case "Bon":
                        if (SelectedTableRow is Bon bon)
                        {
                            UpdateBon(bon.IDbon, bon.data_eliberare, bon.IDcasier, bon.suma_incasata, bon.exista);
                        }
                        break;
                    case "Categorie":
                        if (SelectedTableRow is Categorie categorie)
                        {
                            UpdateCategorie(categorie.IDcategorie, categorie.nume_categorie, categorie.exista);
                        }
                        break;
                    case "Producator":
                        if (SelectedTableRow is Producator producator)
                        {
                            UpdateProducator(producator.IDproducator, producator.nume_producator, producator.tara_origine, producator.exista);
                        }
                        break;
                    case "Produs":
                        if (SelectedTableRow is Produs produs)
                        {
                            UpdateProdus(produs.IDprodus, produs.nume_produs, produs.cod_bare, produs.IDcategorie, produs.IDproducator, produs.exista);
                        }
                        break;
                    case "ProduseVandute":
                        if (SelectedTableRow is ProduseVandute produseVandute)
                        {
                            UpdateProdusVandut(produseVandute.IDbon, produseVandute.IDprodus, produseVandute.cantitate, produseVandute.subtotal, produseVandute.exista);
                        }
                        break;
                    case "Stoc":
                        if (SelectedTableRow is Stoc stoc)
                        {
                            UpdateStoc(stoc.IDstoc, stoc.IDprodus, stoc.cantitate, stoc.unitate_de_masura, stoc.data_aprovizionare, stoc.data_expirare, stoc.pret_vanzare, stoc.exista);
                        }
                        break;
                    case "Utilizator":
                        if (SelectedTableRow is Utilizator utilizator)
                        {
                            UpdateUtilizator(utilizator.IDutilizator, utilizator.nume_utilizator, utilizator.parola, utilizator.tip_utilizator, utilizator.exista);
                        }
                        break;
                    default:
                        break;
                }
            }
        }


        private void UpdateBon(long iDbon, DateTime data_eliberare, int iDcasier, float suma_incasata, bool exista)
        {
            //_context.UpdateBon(iDbon, data_eliberare, iDcasier, suma_incasata, exista);
            //LoadTableData();
            MessageBox.Show("Nu aveti permisiunea de a modifica bonurile", "Eroare", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void UpdateCategorie(int iDcategorie, string nume_categorie, bool exista)
        {
            _context.UpdateCategorie(iDcategorie, nume_categorie, exista);
            LoadTableData();
            MessageBox.Show("Categoria a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateProducator(long iDproducator, string nume_producator, string tara_origine, bool exista)
        {
            _context.UpdateProducator(iDproducator, nume_producator, tara_origine, exista);
            LoadTableData();
            MessageBox.Show("Producatorul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateProdus(long iDprodus, string nume_produs, long cod_bare, int iDcategorie, long iDproducator, bool exista)
        {
            _context.UpdateProdus(iDprodus, nume_produs, cod_bare, iDcategorie, iDproducator, exista);
            LoadTableData();
            MessageBox.Show("Produsul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateProdusVandut(long iDbon, long iDprodus, int cantitate, float subtotal, bool exista)
        {
            //_context.UpdateProdusVandut(iDbon, iDprodus, cantitate, subtotal, exista);
            //LoadTableData();
            MessageBox.Show("Nu aveti permisiunea de a modifica produsele vandute", "Eroare", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateStoc(long iDstoc, long iDprodus, long cantitate, string unitate_de_masura, DateTime data_aprovizionare, DateTime data_expirare, float pret_vanzare, bool exista)
        {
            float? pret_achizitieNullable = _context.GetPretAchizitieByIdProdus(iDprodus);

            if (pret_achizitieNullable.HasValue)
            {
                float pret_achizitie = pret_achizitieNullable.Value;

                if (pret_vanzare >= pret_achizitie)
                {
                    _context.UpdateStoc(iDstoc, iDprodus, cantitate, unitate_de_masura, data_aprovizionare, data_expirare, pret_vanzare, exista);
                    LoadTableData();
                    MessageBox.Show("Stocul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Prețul de vânzare nu poate fi mai mic decât prețul de achiziție!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Prețul de achiziție nu este disponibil pentru produsul selectat!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void UpdateUtilizator(int iDutilizator, string nume_utilizator, string parola, string tip_utilizator, bool exista)
        {
            _context.UpdateUtilizator(iDutilizator, nume_utilizator, parola, tip_utilizator, exista);
            LoadTableData();
            MessageBox.Show("Utilizatorul a fost actualizat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void UpdateBonMethod(object param)
        {
            if (param is Bon bon)
            {
                UpdateBon(bon.IDbon, bon.data_eliberare, bon.IDcasier, bon.suma_incasata, bon.exista);
            }
        }

        private void UpdateCategorieMethod(object param)
        {
            if (param is Categorie c)
            {
                UpdateCategorie(c.IDcategorie, c.nume_categorie, c.exista);
            }
        }

        private void UpdateProducatorMethod(object param)
        {
            if (param is Producator p)
            {
                UpdateProducator(p.IDproducator, p.nume_producator, p.tara_origine, p.exista);
            }
        }

        private void UpdateProdusMethod(object param)
        {
            if (param is Produs produs)
            {
                UpdateProdus(produs.IDprodus, produs.nume_produs, produs.cod_bare, produs.IDcategorie, produs.IDproducator, produs.exista);
            }
        }

        private void UpdateProdusVandutMethod(object param)
        {
            if (param is ProduseVandute pv)
            {
                UpdateProdusVandut(pv.IDbon, pv.IDprodus, pv.cantitate, pv.subtotal, pv.exista);
            }
        }

        private void UpdateStocMethod(object param)
        {
            if (param is Stoc stoc)
            {
                UpdateStoc(stoc.IDstoc, stoc.IDprodus, stoc.cantitate, stoc.unitate_de_masura, stoc.data_aprovizionare, stoc.data_expirare, stoc.pret_vanzare, stoc.exista);
            }
        }

        private void UpdateUtilizatorMethod(object param)
        {
            if (param is Utilizator utilizator)
            {
                UpdateUtilizator(utilizator.IDutilizator, utilizator.nume_utilizator, utilizator.parola, utilizator.tip_utilizator, utilizator.exista);
            }
        }
        private void DisplayLargestReceipt()
        {
            DisplayLargestReceipt(CurrentDate);
        }
        private void DisplayLargestReceipt(DateTime selectedDate)
        {
            var receiptsForDay = _context.Bon
                .Where(b => DbFunctions.TruncateTime(b.data_eliberare) == selectedDate.Date)
                .ToList();


            if (receiptsForDay.Any())
            {
                var largestReceipt = receiptsForDay.OrderByDescending(b => b.suma_incasata).FirstOrDefault();

                MessageBox.Show($"Detalii bon: ID: {largestReceipt.IDbon}, Data: {largestReceipt.data_eliberare}, Suma incasata: {largestReceipt.suma_incasata}", "Cel mai mare bon", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nu exista bonuri pentru data selectata.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string _enteredUsername;
        public string EnteredUsername
        {
            get { return _enteredUsername; }
            set
            {
                _enteredUsername = value;
                OnPropertyChanged(nameof(EnteredUsername));
            }
        }

        public ICommand DisplayUserReceiptsCommand { get; }
        private void DisplayUserReceipts(object param)
        {
            if (param is DateTime selectedDate)
            {
                DisplayUserReceipts(selectedDate);
            }
        }


        private void DisplayUserReceipts(DateTime selectedDate)
        {
            string username = EnteredUsername; 

            var userReceiptsForMonth = _context.Bon
                .Where(b => b.Utilizator.nume_utilizator == username && b.data_eliberare.Year == selectedDate.Year && b.data_eliberare.Month == selectedDate.Month)
                .ToList();

            if (userReceiptsForMonth.Any())
            {
                var receiptsGroupedByDay = userReceiptsForMonth
                    .GroupBy(b => b.data_eliberare.Date)
                    .Select(g => new { Date = g.Key, Total = g.Sum(b => b.suma_incasata) });

                string message = $"Suma încasată de utilizatorul '{username}' în luna {selectedDate.ToString("MMMM yyyy")} pe zile:\n\n";
                foreach (var receipt in receiptsGroupedByDay)
                {
                    message += $"Data: {receipt.Date.ToShortDateString()} - Sumă încasată: {receipt.Total}\n";
                }

                MessageBox.Show(message, "Sumă încasată de utilizator", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Nu există bonuri pentru utilizatorul '{username}' în luna selectată.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
