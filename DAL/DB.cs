using Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
//Code de base venant de l'exemple ContactsJSON - Upgraded fait par Nicolas Chourot //https://github.com/Nicolas-Chourot/ContactsJSON-Upgraded/tree/master
//Code modifié par Alexandre Rochefort Février 2026

namespace DAL
{
    public sealed class DB
    {
        #region singleton setup
        private static readonly DB instance = new DB();
        public static DB Instance { get { return instance; } }
        #endregion

        public static Repository<Media> Medias { get; set; } = new Repository<Media>();

        public static List<string> MediasCategories()
        {
            return Medias.ToList()
                         .Select(m => m.Category)
                         .Where(c => !string.IsNullOrWhiteSpace(c))
                         .Distinct()
                         .OrderBy(c => c)
                         .ToList();
        }
    }
    }