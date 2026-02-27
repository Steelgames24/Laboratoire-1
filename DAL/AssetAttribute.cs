using System;
//Code de base venant de l'exemple ContactsJSON - Upgraded fait par Nicolas Chourot //https://github.com/Nicolas-Chourot/ContactsJSON-Upgraded/tree/master

namespace DAL
{
    public class AssetAttribute : Attribute
    {
        private readonly string folder;

        public AssetAttribute(string folder)
        {
            this.folder = folder;
        }
        public string Folder() => folder;
    }
}