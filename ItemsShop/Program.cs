namespace ItemsShop
{
    class Programm
    {
        static void Main()
        {
            Shop menu = new Shop();
            menu.ActivateShopMenu();
        }
    }

    class Shop
    {
        private const string MenuShowItems = "1";
        private const string MenuBuyItem = "2";
        private const string MenuCheckBag = "3";
        private const string MenuExit = "0";

        private Seller _seller = new Seller(1000);
        private Buyer _buyer = new Buyer(1500);

        public void ActivateShopMenu()
        {
            bool isExit = false;

            string userInput;

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
                        _seller.ShowAllAgentInfo();
                        break;

                    case MenuBuyItem:
                        SellItem();
                        break;

                    case MenuCheckBag:
                        _buyer.ShowAllAgentInfo();
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

            if (CanSell(out item))
            {
                _seller.SellItem(item);
                _buyer.BuyItem(item);
                Console.WriteLine($"Удачная покупка! \"{item.Name}\" теперь у Вас в сумке!");
            }
            else
            {
                Console.WriteLine("Не удалось купить предмет");
            }
        }

        private bool CanSell(out Item item)
        {
            item = null;

            if (_seller.TryGetItem(out item))
            {
                if (_buyer.CanBuy(item.Price))
                {
                    return true;
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

        public void ShowAllAgentInfo()
        {
            ShowMoney();
            ShowBag();
        }

        private void ShowMoney()
        {
            Console.WriteLine("Монеты: " + Money);
        }

        private void ShowBag()
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
            List<string> names = new List<string>(new string[] { "Пятка динозавра", "Жеваный паук", "Коготь овцы", "Рог слизня", "Волосы лысой выхухоли", "Меч из грязи", "Зерно крысы" });
            List<int> prices = new List<int>(new int[] { 100, 15, 30, 200, 150, 500, 250 });

            for (int i = 0; i < names.Count; i++)
            {
                Bag.Add(new Item(++_lastIndex, names[i], prices[i]));
            }
        }

        public void SellItem(Item item)
        {
            Money += item.Price;
            Bag.Remove(item);
        }

        public bool TryGetItem(out Item item)
        {
            item = null;

            if (Bag.Count <= 0)
            {
                Console.WriteLine("Продавец говорит: \"Ты уже купил у меня весь мусор! Вот же простофиля! Хе-хе...\"");
                return false;
            }

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

    class Buyer : Agent
    {
        public Buyer(int money) : base(money)
        {
        }

        public void BuyItem(Item item)
        {
            Bag.Add(item);
            Money -= item.Price;
        }

        public bool CanBuy(int price)
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