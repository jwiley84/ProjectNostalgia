using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Inventory.Scripts.ItemScripts
{
    public class ItemContainer
    {
        private List<Item> weapons = new List<Item>();
        private List<Item> equipment = new List<Item>();
        private List<Item> consumables = new List<Item>();

        public List<Item> Weapons { get => weapons; set => weapons = value; }
        public List<Item> Equipment { get => equipment; set => equipment = value; }
        public List<Item> Consumables { get => consumables; set => consumables = value; }

        public ItemContainer()
        {

        }
    }
}
