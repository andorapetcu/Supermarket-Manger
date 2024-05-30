namespace Tema3MVVM
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;

    public partial class supermarketEntities : DbContext
    {
        public supermarketEntities()
    : base("name=supermarketEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Bon> Bon { get; set; }
        public virtual DbSet<Categorie> Categorie { get; set; }
        public virtual DbSet<Producator> Producator { get; set; }
        public virtual DbSet<Produs> Produs { get; set; }
        public virtual DbSet<ProduseVandute> ProduseVandute { get; set; }
        public virtual DbSet<Stoc> Stoc { get; set; }
        public virtual DbSet<Utilizator> Utilizator { get; set; }

        public virtual int DeleteBon(Nullable<long> iDbon)
        {
            var iDbonParameter = iDbon.HasValue ?
                new ObjectParameter("IDbon", iDbon) :
                new ObjectParameter("IDbon", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteBon", iDbonParameter);
        }

        public virtual int DeleteCategorie(Nullable<int> iDcategorie)
        {
            var iDcategorieParameter = iDcategorie.HasValue ?
                new ObjectParameter("IDcategorie", iDcategorie) :
                new ObjectParameter("IDcategorie", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteCategorie", iDcategorieParameter);
        }

        public virtual int DeleteProducator(Nullable<long> iDproducator)
        {
            var iDproducatorParameter = iDproducator.HasValue ?
                new ObjectParameter("IDproducator", iDproducator) :
                new ObjectParameter("IDproducator", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteProducator", iDproducatorParameter);
        }

        public virtual int DeleteProdus(Nullable<long> iDprodus)
        {
            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteProdus", iDprodusParameter);
        }

        public virtual int DeleteProdusVandut(Nullable<long> iDbon, Nullable<long> iDprodus)
        {
            var iDbonParameter = iDbon.HasValue ?
                new ObjectParameter("IDbon", iDbon) :
                new ObjectParameter("IDbon", typeof(long));

            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteProdusVandut", iDbonParameter, iDprodusParameter);
        }

        public virtual int DeleteStoc(Nullable<long> iDstoc)
        {
            var iDstocParameter = iDstoc.HasValue ?
                new ObjectParameter("IDstoc", iDstoc) :
                new ObjectParameter("IDstoc", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteStoc", iDstocParameter);
        }

        public virtual int DeleteUtilizator(Nullable<int> iDutilizator)
        {
            var iDutilizatorParameter = iDutilizator.HasValue ?
                new ObjectParameter("IDutilizator", iDutilizator) :
                new ObjectParameter("IDutilizator", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DeleteUtilizator", iDutilizatorParameter);
        }

        public virtual int InsertBon(Nullable<System.DateTime> data_eliberare, Nullable<int> iDcasier, Nullable<float> suma_incasata, Nullable<bool> exista)
        {
            var data_eliberareParameter = data_eliberare.HasValue ?
                new ObjectParameter("data_eliberare", data_eliberare) :
                new ObjectParameter("data_eliberare", typeof(System.DateTime));

            var iDcasierParameter = iDcasier.HasValue ?
                new ObjectParameter("IDcasier", iDcasier) :
                new ObjectParameter("IDcasier", typeof(int));

            var suma_incasataParameter = suma_incasata.HasValue ?
                new ObjectParameter("suma_incasata", suma_incasata) :
                new ObjectParameter("suma_incasata", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertBon", data_eliberareParameter, iDcasierParameter, suma_incasataParameter, existaParameter);
        }

        public virtual int InsertCategorie(string nume_categorie, Nullable<bool> exista)
        {
            var nume_categorieParameter = nume_categorie != null ?
                new ObjectParameter("nume_categorie", nume_categorie) :
                new ObjectParameter("nume_categorie", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertCategorie", nume_categorieParameter, existaParameter);
        }

        public virtual int InsertProducator(string nume_producator, string tara_origine, Nullable<bool> exista)
        {
            var nume_producatorParameter = nume_producator != null ?
                new ObjectParameter("nume_producator", nume_producator) :
                new ObjectParameter("nume_producator", typeof(string));

            var tara_origineParameter = tara_origine != null ?
                new ObjectParameter("tara_origine", tara_origine) :
                new ObjectParameter("tara_origine", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertProducator", nume_producatorParameter, tara_origineParameter, existaParameter);
        }

        public virtual int InsertProdus(string nume_produs, Nullable<long> cod_bare, Nullable<int> iDcategorie, Nullable<long> iDproducator, Nullable<bool> exista)
        {
            var nume_produsParameter = nume_produs != null ?
                new ObjectParameter("nume_produs", nume_produs) :
                new ObjectParameter("nume_produs", typeof(string));

            var cod_bareParameter = cod_bare.HasValue ?
                new ObjectParameter("cod_bare", cod_bare) :
                new ObjectParameter("cod_bare", typeof(long));

            var iDcategorieParameter = iDcategorie.HasValue ?
                new ObjectParameter("IDcategorie", iDcategorie) :
                new ObjectParameter("IDcategorie", typeof(int));

            var iDproducatorParameter = iDproducator.HasValue ?
                new ObjectParameter("IDproducator", iDproducator) :
                new ObjectParameter("IDproducator", typeof(long));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertProdus", nume_produsParameter, cod_bareParameter, iDcategorieParameter, iDproducatorParameter, existaParameter);
        }

        public virtual int InsertProdusVandut(Nullable<long> iDbon, Nullable<long> iDprodus, Nullable<int> cantitate, Nullable<float> subtotal, Nullable<bool> exista)
        {
            var iDbonParameter = iDbon.HasValue ?
                new ObjectParameter("IDbon", iDbon) :
                new ObjectParameter("IDbon", typeof(long));

            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            var cantitateParameter = cantitate.HasValue ?
                new ObjectParameter("cantitate", cantitate) :
                new ObjectParameter("cantitate", typeof(int));

            var subtotalParameter = subtotal.HasValue ?
                new ObjectParameter("subtotal", subtotal) :
                new ObjectParameter("subtotal", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertProdusVandut", iDbonParameter, iDprodusParameter, cantitateParameter, subtotalParameter, existaParameter);
        }

        public virtual int InsertStoc(Nullable<long> iDprodus, Nullable<long> cantitate, string unitate_de_masura, Nullable<System.DateTime> data_aprovizionare, Nullable<System.DateTime> data_expirare, Nullable<float> pret_achizitie, Nullable<float> pret_vanzare, Nullable<bool> exista)
        {
            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            var cantitateParameter = cantitate.HasValue ?
                new ObjectParameter("cantitate", cantitate) :
                new ObjectParameter("cantitate", typeof(long));

            var unitate_de_masuraParameter = unitate_de_masura != null ?
                new ObjectParameter("unitate_de_masura", unitate_de_masura) :
                new ObjectParameter("unitate_de_masura", typeof(string));

            var data_aprovizionareParameter = data_aprovizionare.HasValue ?
                new ObjectParameter("data_aprovizionare", data_aprovizionare) :
                new ObjectParameter("data_aprovizionare", typeof(System.DateTime));

            var data_expirareParameter = data_expirare.HasValue ?
                new ObjectParameter("data_expirare", data_expirare) :
                new ObjectParameter("data_expirare", typeof(System.DateTime));

            var pret_achizitieParameter = pret_achizitie.HasValue ?
                new ObjectParameter("pret_achizitie", pret_achizitie) :
                new ObjectParameter("pret_achizitie", typeof(float));

            var pret_vanzareParameter = pret_vanzare.HasValue ?
                new ObjectParameter("pret_vanzare", pret_vanzare) :
                new ObjectParameter("pret_vanzare", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertStoc", iDprodusParameter, cantitateParameter, unitate_de_masuraParameter, data_aprovizionareParameter, data_expirareParameter, pret_achizitieParameter, pret_vanzareParameter, existaParameter);
        }

        public virtual int InsertUtilizator(string nume_utilizator, string parola, string tip_utilizator, Nullable<bool> exista)
        {
            var nume_utilizatorParameter = nume_utilizator != null ?
                new ObjectParameter("nume_utilizator", nume_utilizator) :
                new ObjectParameter("nume_utilizator", typeof(string));

            var parolaParameter = parola != null ?
                new ObjectParameter("parola", parola) :
                new ObjectParameter("parola", typeof(string));

            var tip_utilizatorParameter = tip_utilizator != null ?
                new ObjectParameter("tip_utilizator", tip_utilizator) :
                new ObjectParameter("tip_utilizator", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertUtilizator", nume_utilizatorParameter, parolaParameter, tip_utilizatorParameter, existaParameter);
        }

        public virtual ObjectResult<SelectAllBonuri_Result> SelectAllBonuri()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllBonuri_Result>("SelectAllBonuri");
        }

        public virtual ObjectResult<SelectAllCategorii_Result> SelectAllCategorii()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllCategorii_Result>("SelectAllCategorii");
        }

        public virtual ObjectResult<SelectAllProducatori_Result> SelectAllProducatori()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllProducatori_Result>("SelectAllProducatori");
        }

        public virtual ObjectResult<SelectAllProduse_Result> SelectAllProduse()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllProduse_Result>("SelectAllProduse");
        }

        public virtual ObjectResult<SelectAllProduseVandute_Result> SelectAllProduseVandute()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllProduseVandute_Result>("SelectAllProduseVandute");
        }

        public virtual ObjectResult<SelectAllStoc_Result> SelectAllStoc()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllStoc_Result>("SelectAllStoc");
        }

        public virtual ObjectResult<SelectAllUtilizatori_Result> SelectAllUtilizatori()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SelectAllUtilizatori_Result>("SelectAllUtilizatori");
        }

        public virtual int UpdateBon(Nullable<long> iDbon, Nullable<System.DateTime> data_eliberare, Nullable<int> iDcasier, Nullable<float> suma_incasata, Nullable<bool> exista)
        {
            var iDbonParameter = iDbon.HasValue ?
                new ObjectParameter("IDbon", iDbon) :
                new ObjectParameter("IDbon", typeof(long));

            var data_eliberareParameter = data_eliberare.HasValue ?
                new ObjectParameter("data_eliberare", data_eliberare) :
                new ObjectParameter("data_eliberare", typeof(System.DateTime));

            var iDcasierParameter = iDcasier.HasValue ?
                new ObjectParameter("IDcasier", iDcasier) :
                new ObjectParameter("IDcasier", typeof(int));

            var suma_incasataParameter = suma_incasata.HasValue ?
                new ObjectParameter("suma_incasata", suma_incasata) :
                new ObjectParameter("suma_incasata", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateBon", iDbonParameter, data_eliberareParameter, iDcasierParameter, suma_incasataParameter, existaParameter);
        }

        public virtual int UpdateCategorie(Nullable<int> iDcategorie, string nume_categorie, Nullable<bool> exista)
        {
            var iDcategorieParameter = iDcategorie.HasValue ?
                new ObjectParameter("IDcategorie", iDcategorie) :
                new ObjectParameter("IDcategorie", typeof(int));

            var nume_categorieParameter = nume_categorie != null ?
                new ObjectParameter("nume_categorie", nume_categorie) :
                new ObjectParameter("nume_categorie", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateCategorie", iDcategorieParameter, nume_categorieParameter, existaParameter);
        }

        public virtual int UpdateProducator(Nullable<long> iDproducator, string nume_producator, string tara_origine, Nullable<bool> exista)
        {
            var iDproducatorParameter = iDproducator.HasValue ?
                new ObjectParameter("IDproducator", iDproducator) :
                new ObjectParameter("IDproducator", typeof(long));

            var nume_producatorParameter = nume_producator != null ?
                new ObjectParameter("nume_producator", nume_producator) :
                new ObjectParameter("nume_producator", typeof(string));

            var tara_origineParameter = tara_origine != null ?
                new ObjectParameter("tara_origine", tara_origine) :
                new ObjectParameter("tara_origine", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateProducator", iDproducatorParameter, nume_producatorParameter, tara_origineParameter, existaParameter);
        }

        public virtual int UpdateProdus(Nullable<long> iDprodus, string nume_produs, Nullable<long> cod_bare, Nullable<int> iDcategorie, Nullable<long> iDproducator, Nullable<bool> exista)
        {
            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            var nume_produsParameter = nume_produs != null ?
                new ObjectParameter("nume_produs", nume_produs) :
                new ObjectParameter("nume_produs", typeof(string));

            var cod_bareParameter = cod_bare.HasValue ?
                new ObjectParameter("cod_bare", cod_bare) :
                new ObjectParameter("cod_bare", typeof(long));

            var iDcategorieParameter = iDcategorie.HasValue ?
                new ObjectParameter("IDcategorie", iDcategorie) :
                new ObjectParameter("IDcategorie", typeof(int));

            var iDproducatorParameter = iDproducator.HasValue ?
                new ObjectParameter("IDproducator", iDproducator) :
                new ObjectParameter("IDproducator", typeof(long));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateProdus", iDprodusParameter, nume_produsParameter, cod_bareParameter, iDcategorieParameter, iDproducatorParameter, existaParameter);
        }

        public virtual int UpdateProdusVandut(Nullable<long> iDbon, Nullable<long> iDprodus, Nullable<int> cantitate, Nullable<float> subtotal, Nullable<bool> exista)
        {
            var iDbonParameter = iDbon.HasValue ?
                new ObjectParameter("IDbon", iDbon) :
                new ObjectParameter("IDbon", typeof(long));

            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            var cantitateParameter = cantitate.HasValue ?
                new ObjectParameter("cantitate", cantitate) :
                new ObjectParameter("cantitate", typeof(int));

            var subtotalParameter = subtotal.HasValue ?
                new ObjectParameter("subtotal", subtotal) :
                new ObjectParameter("subtotal", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateProdusVandut", iDbonParameter, iDprodusParameter, cantitateParameter, subtotalParameter, existaParameter);
        }

        public virtual int UpdateStoc(Nullable<long> iDstoc, Nullable<long> iDprodus, Nullable<long> cantitate, string unitate_de_masura, Nullable<System.DateTime> data_aprovizionare, Nullable<System.DateTime> data_expirare, Nullable<float> pret_vanzare, Nullable<bool> exista)
        {
            var iDstocParameter = iDstoc.HasValue ?
                new ObjectParameter("IDstoc", iDstoc) :
                new ObjectParameter("IDstoc", typeof(long));

            var iDprodusParameter = iDprodus.HasValue ?
                new ObjectParameter("IDprodus", iDprodus) :
                new ObjectParameter("IDprodus", typeof(long));

            var cantitateParameter = cantitate.HasValue ?
                new ObjectParameter("cantitate", cantitate) :
                new ObjectParameter("cantitate", typeof(long));

            var unitate_de_masuraParameter = unitate_de_masura != null ?
                new ObjectParameter("unitate_de_masura", unitate_de_masura) :
                new ObjectParameter("unitate_de_masura", typeof(string));

            var data_aprovizionareParameter = data_aprovizionare.HasValue ?
                new ObjectParameter("data_aprovizionare", data_aprovizionare) :
                new ObjectParameter("data_aprovizionare", typeof(System.DateTime));

            var data_expirareParameter = data_expirare.HasValue ?
                new ObjectParameter("data_expirare", data_expirare) :
                new ObjectParameter("data_expirare", typeof(System.DateTime));

            var pret_vanzareParameter = pret_vanzare.HasValue ?
                new ObjectParameter("pret_vanzare", pret_vanzare) :
                new ObjectParameter("pret_vanzare", typeof(float));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateStoc", iDstocParameter, iDprodusParameter, cantitateParameter, unitate_de_masuraParameter, data_aprovizionareParameter, data_expirareParameter, pret_vanzareParameter, existaParameter);
        }

        public virtual int UpdateUtilizator(Nullable<int> iDutilizator, string nume_utilizator, string parola, string tip_utilizator, Nullable<bool> exista)
        {
            var iDutilizatorParameter = iDutilizator.HasValue ?
                new ObjectParameter("IDutilizator", iDutilizator) :
                new ObjectParameter("IDutilizator", typeof(int));

            var nume_utilizatorParameter = nume_utilizator != null ?
                new ObjectParameter("nume_utilizator", nume_utilizator) :
                new ObjectParameter("nume_utilizator", typeof(string));

            var parolaParameter = parola != null ?
                new ObjectParameter("parola", parola) :
                new ObjectParameter("parola", typeof(string));

            var tip_utilizatorParameter = tip_utilizator != null ?
                new ObjectParameter("tip_utilizator", tip_utilizator) :
                new ObjectParameter("tip_utilizator", typeof(string));

            var existaParameter = exista.HasValue ?
                new ObjectParameter("exista", exista) :
                new ObjectParameter("exista", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateUtilizator", iDutilizatorParameter, nume_utilizatorParameter, parolaParameter, tip_utilizatorParameter, existaParameter);
        }

        public virtual float? GetPretAchizitieByIdProdus(long iDprodus)
        {
            var iDprodusParameter = new ObjectParameter("IDprodus", iDprodus);

            var pretAchizitieParameter = new ObjectParameter("pret_achizitie", typeof(float));

            var result = ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetPretAchizitieByIdProdus", iDprodusParameter, pretAchizitieParameter);

            if (result == -1)
            {
                return (float?)pretAchizitieParameter.Value;
            }
            else
            {
                return null;
            }
        }

    }
}
