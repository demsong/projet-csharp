using System;
using Products;

namespace Gestion
{
    public enum Etat
    {
        DISPONIBLE,
        EN_RUPTURE,
        EN_REAPPROVISIONNEMENT
    }
    class Program
    {     
        private const int taillePermiseNomProduit = 44;
        private const int taillePermiseStock = 10;
        private const int taillePermiseEtat = 24;
        static void Main(String[] args)
        {
            int n = 1;
            List<Product> produits = loadProductsIntoList("./produits.txt");
            do{
                affichage(produits);

                // On entend que l'utilisatuer entre un id au clavier.
                //Console.WriteLine("Type something: ");
                var la = Console.ReadKey();
                
                int productIdToUpdateStock;
                bool temoin = int.TryParse(la.KeyChar.ToString(), out productIdToUpdateStock);
                if (temoin == true) {
                    // if n has been convert to int, We will update the stock of product which id equals n.
                    foreach(var p in produits)
                    {
                        if(p.Id == productIdToUpdateStock)
                        {
                            p.Stock--;
                            if (p.Stock <= 0) p.Stock = 0;
                            p.Etat = etatExact(p.Stock);
                        }
                    }
                    if (productIdToUpdateStock == 0)
                    {
                        Environment.Exit(0);
                    }
                } 
                Console.Clear();
            } while(n != 0);
        } 

        /**
            * Cette fonction retourne une liste contenant les ids des produits en cours.
            * Afin de mettre à jour les stocks des différents produits en juste saisissant
            * l'id d'un produit.
        */
        static List<int> registerAvailableProductIds(List<Product> products)
        {
            var ids = new List<int>();

            foreach (var item in products)
            {
                ids.Add(item.Id);
            }
            return ids;
        }

        /**
            * Cette méthode permet de charger les enregistrements contenus dans
            * le fichier produits.txt. Un enregistrement représente un produit.  
        */        
        static List<Product> loadProductsIntoList(string filePath)
        {
            var lineProductsArray = File.ReadAllLines(filePath).ToList();
            var products = new List<Product>();

            int id;
            int stock;
            string name;
            Etat etat;
            string[] productDataArray;

            foreach( var line in lineProductsArray)
            {
                productDataArray = line.Split(":");

                int.TryParse(productDataArray[0], out id);
                name = productDataArray[1];

                int.TryParse(productDataArray[2], out stock);
                etat = etatExact(stock);

                Product p = new Product(id, name, stock, etat);
                products.Add(p);
            }
            return products;
        }
        
        /**
            * Cette méthode prend en paramètre un objet Etat et
            * retourne sa valeur en chaine de caractères.
        */
        static string matchEtatToString(Etat etat)
        {
            string state;

            switch (etat)
            {
                case Etat.DISPONIBLE:
                    state = "Disponible";
                break;
                case Etat.EN_RUPTURE:
                    state = "Rupture";
                break;
                case Etat.EN_REAPPROVISIONNEMENT:
                    state = "Réapprovisionnement";
                break;
                default: state = "Rupture";
                break;
            }
            return state;
        }

        /**
            * n fonction du stock d'un produit, 
            * son état exact est retourné.
        */
        static Etat etatExact(int quantite)
        {
            Etat state = Etat.EN_RUPTURE;
            
            if (quantite > 5) state = Etat.DISPONIBLE;
            if (quantite <= 5) state = Etat.EN_REAPPROVISIONNEMENT;
            if (quantite <= 0) state = Etat.EN_RUPTURE;
            
            return state;
        }

        /**
            * Cette méthode réalise l'affichage 
            * des produits dans le tableau. 
        */
        static void affichage(List<Product> produits)
        {
            Console.WriteLine("\n");
            Console.WriteLine("+...+...............+.............+..............+............+..........+...............+");
            Console.WriteLine("| # | Produits                                   |    Stocks  |            Etat          |");
            Console.WriteLine("+---+............................................+............+..........................+");

            foreach (var item in produits)
            {
                string id = "";
                if(item.Id<10) {
                    id ="| "+item.Id+" | ";
                }else {
                    id ="|"+item.Id+" | ";
                }
                Console.Write(id);  
                
                Console.Write(item.Name);
                afficherEspaceNFois(taillePermiseNomProduit - item.Name.Length - 1);
                Console.Write("| ");
                
                string qteStr = item.Stock.ToString();
                Console.Write(qteStr+" ");
                afficherEspaceNFois(taillePermiseStock - qteStr.Length);
                Console.Write("| ");
                string etatStock = matchEtatToString(item.Etat);
                Console.Write(etatStock);
                afficherEspaceNFois(taillePermiseEtat - etatStock.Length);
                Console.Write("|\n");
            }
            Console.WriteLine("+---+--------------------------------------------+------------+-------------------------+");
        }

        /**
            * Cette méthode est nécessaire lors de 
            * l'affichage des produits dans le tableau.
        */
        static void afficherEspaceNFois(int n)
        {
            for (int i = 0; i<n; i++) Console.Write(" ");
        }
	}
}