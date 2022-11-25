namespace ItemsShop
{
    class Programm
    {
        static void Main()
        {
            Shop menu = new Shop();
            menu.ShowMainMenu();
        }
    }

    class Shop
    {
        private const string MenuShowItems = "1";
        private const string MenuBuyItem = "2";
        private const string MenuCheckBag = "3";
        private const string MenuExit = "0";

        Seller seller = new Seller(1000);
        Byer byer = new Byer(1000);

        public void ShowMainMenu()
        {
            bool isExit = false;
            string userInput;

            seller.CreateSampleItems();

            while (isExit == false)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine(MenuShowItems + " - Показыть товары продавца");
                Console.WriteLine(MenuBuyItem + " - Купить товар");
                Console.WriteLine(MenuCheckBag + " - Проверить свой мешок");
                Console.WriteLine(MenuExit + " - Выход");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case MenuShowItems:
                        seller.ShowMoney();
                        seller.ShowAllItems();
                        break;

                    case MenuBuyItem:
                        CanSellItem();
                        break;

                    case MenuCheckBag:
                        byer.ShowMoney();
                        byer.ShowBag();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }

        private void CanSellItem()
        {
            Item item;

            if (SellCheck(out item))
            {
                seller.RemoveItem(item);
                seller.TakeMoney(byer.BuyItem(item));
                Console.WriteLine($"Удачная покупка! \"{item.Name}\" теперь у Вас в сумке!");
            }
            else
            {
                Console.WriteLine("Не удалось купить предмет");
            }
        }

        private bool SellCheck(out Item item)
        {
            item = null;

            if (seller.CheckItemsAvailable())
            {
                if (seller.TryGetItem(out item))
                {
                    if (byer.CanBuy(item.Price))
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }
    }

    class Seller
    {
        private int _money;
        private int _lastIndex;

        private List<Item> _items = new List<Item>();
        private List<string> _names = new List<string>(new string[] { "Пятка динозавра", "Жеваный паук", "Коготь овцы", "Рог слизня", "Волосы лысой выхухоли", "Меч из грязи", "Зерно крысы" });
        private List<int> _prices = new List<int>(new int[] { 100, 15, 30, 200, 150, 500, 250 });

        public void CreateSampleItems()
        {
            for (int i = 0; i < _names.Count; i++)
            {
                _items.Add(new Item(++_lastIndex, _names[i], _prices[i]));
            }
        }

        public void TakeMoney(int money)
        {
            _money = _money + money;
        }

        public void ShowMoney()
        {
            Console.WriteLine("Монет у продавца: " + _money);
        }

        public void ShowAllItems()
        {
            if (_items.Count > 0)
            {
                foreach (var item in _items)
                {
                    ItemInfo(item);
                }
            }
            else
            {
                Console.WriteLine("Товар кончился");
            }
        }

        private void ItemInfo(Item item)
        {
            Console.WriteLine($"Индекс: {item.Id} | Товар: {item.Name} | Цена: {item.Price}");
        }
        
        public bool TryGetItem(out Item item)
        {
            item = null;

            Console.WriteLine("Введите Id предмета:");
            int id = GetNumber();

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == id)
                {
                    item = _items[i];
                    return true;
                }
            }

            Console.WriteLine("Предмета с таким Id не существует");
            return false;
        }

        public bool CheckItemsAvailable()
        {
            if (_items.Count > 0)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Нет товаров");
                return false;
            }
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
        }

        private int GetNumber()
        {
            int parsedNumber = 0;

            bool isParsed = false;

            while (isParsed == false)
            {
                string userInput = Console.ReadLine();
                isParsed = int.TryParse(userInput, out parsedNumber);

                if (isParsed == false)
                {
                    Console.WriteLine("Введите целое число:");
                }
            }

            return parsedNumber;
        }

        public Seller(int money)
        {
            _money = money;
        }
    }

    class Item
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Price { get; private set; }

        public Item(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }

    class Byer
    {
        private int _money = 0;

        private List<Item> _bag = new List<Item>();

        public Byer(int money)
        {
            _money = money;
        }

        public void ShowMoney()
        {
            Console.WriteLine("Ваши монеты: " + _money);
        }

        public int BuyItem(Item item)
        {
            _bag.Add(item);
            _money = _money - item.Price;
            return item.Price;
        }

        public bool CanBuy(int price)
        {
            if (price < _money)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Не хватает денег =( ");
                return false;
            }
        }

        public void ShowBag()
        {
            if (_bag.Count > 0)
            {
                foreach (var item in _bag)
                {
                    ItemInfo(item);
                }
            }
            else
            {
                Console.WriteLine("В сумке пусто");
            }
        }

        private void ItemInfo(Item item)
        {
            Console.WriteLine($"Индекс: {item.Id} | Товар: {item.Name} | Цена: {item.Price}");
        }
    }
}