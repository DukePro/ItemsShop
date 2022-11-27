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
                        _seller.ShowAllItems();
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
                    if (_byer.CheckBuy(item.Price))
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
        protected int _money;

        protected List<Item> _bag = new List<Item>();

        public Agent(int money)
        {
            _money = money;
        }
    }

    class Seller : Agent
    {
        private int _lastIndex;

        private List<string> _names = new List<string>(new string[] { "Пятка динозавра", "Жеваный паук", "Коготь овцы", "Рог слизня", "Волосы лысой выхухоли", "Меч из грязи", "Зерно крысы" });
        private List<int> _prices = new List<int>(new int[] { 100, 15, 30, 200, 150, 500, 250 });

        public Seller(int money) : base(money)
        {

        }

        public void CreateSampleItems()
        {
            for (int i = 0; i < _names.Count; i++)
            {
                _bag.Add(new Item(++_lastIndex, _names[i], _prices[i]));
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
            if (_bag.Count > 0)
            {
                foreach (var item in _bag)
                {
                    ItemInfo(item);
                }
            }
            else
            {
                Console.WriteLine("Товар кончился");
            }
        }

        public bool TryGetItem(out Item item)
        {
            item = null;

            Console.WriteLine("Введите Id предмета:");
            int id = GetNumber();

            for (int i = 0; i < _bag.Count; i++)
            {
                if (_bag[i].Id == id)
                {
                    item = _bag[i];
                    return true;
                }
            }

            Console.WriteLine("Предмета с таким Id не существует");
            return false;
        }

        public bool CheckItemsAvailable()
        {
            if (_bag.Count > 0)
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
            _bag.Remove(item);
        }

        private void ItemInfo(Item item)
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
    }

    class Byer : Agent
    {
        public Byer(int money) : base(money)
        {

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

        public bool CheckBuy(int price)
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