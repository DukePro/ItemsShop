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

        private Seller _seller = new Seller(1000);
        private Byer _byer = new Byer(1000);

        public void ShowMainMenu()
        {
            bool isExit = false;

            string userInput;

            _seller.CreateSampleItems();

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
                        _seller.ShowMoney();
                        _seller.ShowBag();
                        break;

                    case MenuBuyItem:
                        SellItem();
                        break;

                    case MenuCheckBag:
                        _byer.ShowMoney();
                        _byer.ShowBag();
                        break;

                    case MenuExit:
                        isExit = true;
                        break;
                }
            }
        }

        private void SellItem()
        {
            Item item;

            if (CheckSell(out item))
            {
                _seller.RemoveItem(item);
                _seller.TakeMoney(_byer.BuyItem(item));
                Console.WriteLine($"Удачная покупка! \"{item.Name}\" теперь у Вас в сумке!");
            }
            else
            {
                Console.WriteLine("Не удалось купить предмет");
            }
        }

        private bool CheckSell(out Item item)
        {
            item = null;

            if (_seller.CheckItemsAvailable())
            {
                if (_seller.TryGetItem(out item))
                {
                    if (_byer.CheckCanBuy(item.Price))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    class Agent
    {
        protected int Money;

        protected List<Item> Bag = new List<Item>();

        public Agent(int money)
        {
            Money = money;
        }

        public void ShowMoney()
        {
            Console.WriteLine("Монеты: " + Money);
        }

        public void ShowBag()
        {
            if (Bag.Count > 0)
            {
                foreach (Item item in Bag)
                {
                    item.ShowItemInfo(item);
                }
            }
            else
            {
                Console.WriteLine("Нет товаров");
            }
        }
    }

    class Seller : Agent
    {
        private int _lastIndex;

        public Seller(int money) : base(money)
        {
        }

        public void CreateSampleItems()
        {
        List<string> names = new List<string>(new string[] { "Пятка динозавра", "Жеваный паук", "Коготь овцы", "Рог слизня", "Волосы лысой выхухоли", "Меч из грязи", "Зерно крысы" });
        List<int> prices = new List<int>(new int[] { 100, 15, 30, 200, 150, 500, 250 });

            for (int i = 0; i < names.Count; i++)
            {
                Bag.Add(new Item(++_lastIndex, names[i], prices[i]));
            }
        }

        public void TakeMoney(int money)
        {
            Money = Money + money;
        }

        public bool TryGetItem(out Item item)
        {
            item = null;

            Console.WriteLine("Введите Id предмета:");
            int id = GetNumber();

            for (int i = 0; i < Bag.Count; i++)
            {
                if (Bag[i].Id == id)
                {
                    item = Bag[i];
                    return true;
                }
            }

            Console.WriteLine("Предмета с таким Id не существует");
            return false;
        }

        public bool CheckItemsAvailable()
        {
            if (Bag.Count > 0)
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
            Bag.Remove(item);
        }

        private void ShowItemInfo(Item item)
        {
            Console.WriteLine($"Индекс: {item.Id} | Товар: {item.Name} | Цена: {item.Price}");
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

        public void ShowItemInfo(Item item)
        {
            Console.WriteLine($"Индекс: {item.Id} | Товар: {item.Name} | Цена: {item.Price}");
        }
    }

    class Byer : Agent
    {
        public Byer(int money) : base(money) 
        {
        }

        public int BuyItem(Item item)
        {
            Bag.Add(item);
            Money = Money - item.Price;
            return item.Price;
        }

        public bool CheckCanBuy(int price)
        {
            if (price < Money)
            {
                return true;
            }
            else
            {
                Console.WriteLine("Не хватает денег =( ");
                return false;
            }
        }
    }
}