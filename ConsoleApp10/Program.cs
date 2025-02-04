using System;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ConsoleApp10
{
    public class ICharacter
    {
        public string Name { get; set; }
        public string job { get; set; }
        public int Level { get; set; } //레벨
        public int Ex { get; set; } //경험치
        public int MaxEx { get; set; } //최대 경험치
        public float Health { get; set; } //현제체력
        public int MaxHealth { get; set; } //최대체력
        public int AddHealth { get; set; } = 0;//추가 체력
        public int Attack { get; set; } //공격력
        public int itemAttack { get; set; } = 0; //무기 공격력
        public int AddAttack { get; set; } = 0; //추가 공격력
        public int Defence { get; set; } //방어력
        public int itemDefence { get; set; } = 0; //장비 방어력
        public int Gold { get; set; } //가진 돈
        public bool IsDead { get; set; } //생사

        public void TakeDamage(int damage)
        {
            // 캐릭터가 데미지를 받는 것을 구현
            Health -= damage - (Defence + itemDefence);

            if (Health <= 0)
            {
                Health = 0;
                Console.WriteLine("{0}가 사망했습니다.\n", Name);
                IsDead = true;
            }
        }
        public void TakeHeal(int heal)
        {
            // 캐릭터가 체력이 회복되는 것을 구현
            Console.WriteLine("{0}이 {1}의 회복을 받았습니다.", Name, heal);
            Health += heal;

            if (Health >= MaxHealth + AddHealth)
            {
                Health = MaxHealth + AddHealth; //최대 채력이 넘는 힐은 불가
            }
        }
    }
    public class Player : ICharacter
    {
        public NormalSword normalsord = new NormalSword();
        public IronSword IronSword = new IronSword();
        public NormalSuit normalSuit = new NormalSuit();
        public IronSuit ironSuit = new IronSuit();

        public HpPotion hpPotion = new HpPotion();
        public StPotion stPotion = new StPotion();

        public Player()
        {
            Name = "플레이어";
            job = "전사";

            Level = 1;
            Ex = 0;
            MaxEx = 50;

            Health = 100;
            MaxHealth = 100;

            Gold = 600;

            Attack = 10;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 5;
            itemDefence = 0;

            IsDead = false;
        }
    }
    public class Item
    {
        public string Name { get; set; }
        public string explain { get; set; }
        public int cost { get; set; }
    }
    public class Equip : Item
    {
        public int AddDamage { get; set; }
        public int AddHealth { get; set; }
        public int AddDefence { get; set; }
        public bool PlayerHave { get; set; }
        public bool PlayerUse { get; set; }
        public void Use(Player P)
        {
            if (PlayerUse)
            {
                Console.WriteLine("{0}을(를) 해제했습니다", Name);
                PlayerUse = false;
                P.itemAttack -= AddDamage;
                P.AddHealth -= AddHealth;
                P.itemDefence -= AddDefence;
            }
            else
            {
                Console.WriteLine("{0}을(를) 장착했습니다.", Name);
                PlayerUse = true;
                P.itemAttack += AddDamage;
                P.AddHealth += AddHealth;
                P.itemDefence += AddDefence;
            }
        }
    }
    public class NormalSword : Equip
    {
        public NormalSword()
        {
            Name = "기본 검";
            explain = "기본적인 무딘 검이다.";
            AddDamage = 5;
            AddHealth = 0;
            AddDefence = 0;
            cost = 100;
            PlayerHave = false;
            PlayerUse = false;
        }
    }
    public class IronSword : Equip
    {
        public IronSword()
        {
            Name = "철 검";
            explain = "철로 만든 검이다.";
            AddDamage = 10;
            AddHealth = 0;
            AddDefence = 0;
            cost = 300;
            PlayerHave = false;
            PlayerUse = false;
        }
    }
    public class NormalSuit : Equip
    {
        public NormalSuit()
        {
            Name = "기본 옷";
            explain = "기본적인 평범한 옷이다.";
            AddDamage = 0;
            AddHealth = 10;
            AddDefence = 3;
            cost = 130;
            PlayerHave = false;
            PlayerUse = false;
        }
    }
    public class IronSuit : Equip
    {
        public IronSuit()
        {
            Name = "철 갑옷";
            explain = "철로 만든 튼튼한 갑옷이다이다.";
            AddDamage = 0;
            AddHealth = 30;
            AddDefence = 7;
            cost = 400;
            PlayerHave = false;
            PlayerUse = false;
        }
    }
    public class Potion : Item
    {
        public int PlayerHave { get; set; }
        public void Use(Player P)
        {
            if (PlayerHave <= 0)
            {
                //플레이어가 가지고 있지 않는다면
                Console.WriteLine("가지고 있지 않습니다.");
            }
            else
            {
                // 플레이어가 사용
                switch (Name)
                {
                    case "회복 포션":
                        Console.WriteLine("회복 포션을 사용하여 피를 30만큼 회복했습니다.");
                        P.TakeHeal(30); //30만큼 회복
                        break;
                    case "공격력 포션":
                        Console.WriteLine("공격력 포션을 사용하여 공격력이 10만큼 잠시 증가했습니다.");
                        P.AddAttack = 10; //10만큼 공격력 증가
                        break;
                }
                PlayerHave--;
            }
        }
        public void Get()
        {
            PlayerHave++; //획득 시
            Console.WriteLine("플레이어는 {0}을(를) 얻었습니다.\n", Name);
        }
    }
    public class HpPotion : Potion
    {
        public HpPotion()
        {
            Name = "회복 포션";
            explain = "체력을 30만큼 회복합니다.";
            cost = 50;
            PlayerHave = 0;
        }
    }
    public class StPotion : Potion
    {
        public StPotion()
        {
            Name = "공격력 포션";
            explain = "공격력을 일시적으로 10증가시킵니다.(1전투간)";
            cost = 70;
            PlayerHave = 0;
        }
    }

    public class PlayerInput
    {
        public int Input(int i) //1-i까지의 숫자를 입력 이외에는 "잘못된 입력입니다."
        {
            Console.Write("입력 : ");
            string Pinput = Console.ReadLine();
            int numinput;

            if (int.TryParse(Pinput, out numinput)) //Pinput => int | 1. true | 2. out numinput = int.Parse(Pinput)
            {
                if (numinput >= 1 && numinput <= i)
                {
                    return numinput;
                }
            }
            Console.WriteLine("잘못된 입력입니다.");
            return 0;
        }
    }
    public class MakeUI
    {
        public PlayerInput Input = new PlayerInput();
        public void checkstate(ICharacter C)
        {
            Console.WriteLine("이름 : {0}", C.Name);
            Console.WriteLine("직업 : {0}", C.job);
            Console.WriteLine("레벨 : {0}", C.Level);
            Console.WriteLine("경험치 : {0} / {1}", C.Ex, C.MaxEx);
            Console.WriteLine("소유 골드 : {0}$", C.Gold);

            Console.Write("체력 : {0} / {1}", C.Health, C.MaxHealth + C.AddHealth);
            if(C.AddHealth > 0)
            {
                Console.Write(" ( + {0})", C.AddHealth);
            }
            Console.WriteLine();

            Console.Write("공격력 : {0}", C.Attack + C.AddAttack + C.itemAttack);
            if (C.itemAttack > 0) //무기 공격력
            {
                Console.Write(" ( + {0})", C.itemAttack);
            }
            if (C.AddAttack > 0) //물약 및 버프 공격력
            {
                Console.Write(" ( + {0})", C.AddAttack);
            }
            Console.WriteLine();

            Console.Write("방어력 : {0}", C.Defence + C.itemDefence);
            if (C.itemDefence > 0) //무기 공격력
            {
                Console.Write(" ( + {0})", C.itemDefence);
            }
            Console.WriteLine();

            Console.WriteLine("아무키나 입력하세요.....");
            Console.ReadKey();
        }
        public void checkEquipinven(Player P ,bool use ,bool ex)
        {
            Equip[] Phave = new Equip[10];
            int[] numequip = new int[10];
            int num = 0;

            if (P.normalsord.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.normalsord;
                numequip[num] = num + 1;
                num++;
            }
            if (P.IronSword.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.IronSword;
                numequip[num] = num + 1;
                num++;
            }
            if (P.normalSuit.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.normalSuit;
                numequip[num] = num + 1;
                num++;
            }
            if (P.ironSuit.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.ironSuit;
                numequip[num] = num + 1;
                num++;
            }

            Console.WriteLine("장비창");
            if (ex) //설명을 할것인가
            {
                for (int i = 0; i < num; i++)
                {
                    Console.WriteLine("{0}. {1} : {2}", numequip[i], Phave[i].Name, Phave[i].explain);
                }
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    if (Phave[i].PlayerUse)
                    {
                        Console.WriteLine("{0}. {1} : 착용 O  |  체력 +{2}  |  공격력 +{3}  |  방어력 +{4}", numequip[i], "[E]" + Phave[i].Name, Phave[i].AddHealth, Phave[i].AddDamage, Phave[i].AddDefence);
                    }
                    else
                    {
                        Console.WriteLine("{0}. {1} : 착용 X  |  체력 +{2}  |  공격력 +{3}  |  방어력 +{4}", numequip[i], Phave[i].Name, Phave[i].AddHealth, Phave[i].AddDamage, Phave[i].AddDefence);
                    }
                }
            }

            if (use) //사용할 창으로
            {
                Console.WriteLine("{0}. 돌아가기\n",num+1);
                Console.WriteLine("각 아이템의 숫자를 입력 시 착용/해제 됩니다.\n");
                int j = 0;
                while (true)
                {
                    j = Input.Input(num+1); //1~num 숫자 입력 받기
                    if (j != 0)
                    {
                        break;
                    }
                }
                if(j != num + 1) //돌아가기 선택시 Home()으로
                {
                    Phave[j-1].Use(P); // 착용or해제
                    Console.Clear();
                    checkEquipinven(P, true, false); //계속 장비 선택이 가능하도록
                }
            }
        }
        public void checkuseinven(Player P, bool use, bool ex)
        {
            int i = 1;
            Console.WriteLine("소비 아이템 창");
            Potion[] potions = new Potion[10];
            potions[i-1] = P.hpPotion;
            i++; //2
            potions[i - 1] = P.stPotion;
            i++; //3

            if (ex) //설명을 할것인가
            {
                for (int h = 0; h < i-1; h++)
                {
                    Console.WriteLine("{0}. {1} : {2} ", h + 1, potions[h].Name, potions[h].explain);
                }
            }
            else
            {
                for (int h = 0; h < i-1; h++)
                {
                    Console.WriteLine("{0}. {1} : {2} ", h + 1, potions[h].Name, potions[h].PlayerHave);
                }
            }

            if (use) //사용할것인가
            {
                Console.WriteLine("{0}. 돌아가기\n", i);
                Console.WriteLine("각 아이템의 숫자를 입력 시 사용됩니다.\n");
                int j = 0;
                while (true)
                {
                    j = Input.Input(i); //1~num 숫자 입력 받기
                    if (j != 0)
                    {
                        break;
                    }
                }
                if (j != i) //돌아가기 선택시 Home()으로
                {
                    Console.Clear();
                    potions[j - 1].Use(P); // 사용
                    checkuseinven(P, true, false); //계속 포션 선택이 가능하도록
                }
            }
        }
        public void equipstore(Player P)
        {
            Console.Clear();
            Console.WriteLine("현제 위치 : 상점\n\n");
            Console.WriteLine("상점에 들어왔습니다.\n");
            Console.WriteLine("{0}이(가) 가지고 있는 골드 : {1}$\n", P.Name, P.Gold);
            int i = 0;
            Equip[] equips = new Equip[10];

            Console.WriteLine("상점 품목");

            equips[i] = P.normalsord;
            isPlayerHave(equips[i], i + 1);
            i++;
            equips[i] = P.IronSword;
            isPlayerHave(equips[i], i + 1);
            i++;
            equips[i] = P.normalSuit;
            isPlayerHave(equips[i], i + 1);
            i++;
            equips[i] = P.ironSuit;
            isPlayerHave(equips[i], i + 1);
            i++;

            Console.WriteLine("{0} . 돌아가기\n", i+1);

            Console.WriteLine("원하는 상품의 번호를 입력해 주세요.\n");
            int j = 0;
            while (true)
            {
                j = Input.Input(i+1); //1~i 숫자 입력 받기
                if (j != 0)
                {
                    break;
                }
            }

            if(j != i + 1)//돌아가기 누를 시 마을로
            {
                if (equips[j - 1].PlayerHave) //가지고 있다면
                {
                    Console.WriteLine("{0}은(는) 이미 이 장비를 보유 중입니다.\n", P.Name);
                }
                else if (P.Gold < equips[j-1].cost) //골드가 부족하면
                {
                    Console.WriteLine("{0}이(가) 가지고 있는 골드가 {1}보다 적습니다.\n", P.Name, equips[j - 1].cost);
                }
                else //구매
                {
                    Console.WriteLine("{0}은(는) {1}의 골드를 내고 {2}을(를) 구입했습니다.\n", P.Name, equips[j - 1].cost,equips[j - 1].Name);
                    P.Gold -= equips[j - 1].cost;
                    equips[j-1].PlayerHave = true;
                }
                Console.WriteLine("아무키나 입력하세요.....");
                Console.ReadKey();
                equipstore(P); //구매 후 상점에 머물기
            }
        }
        public void isPlayerHave(Equip E, int i)
        {
            if (E.PlayerHave)
            {
                Console.WriteLine("{0}. {1} : 보유 중  |  체력 +{2}  |  공격력 +{3}  |  방어력 +{4}", i,E.Name,E.AddHealth,E.AddDamage,E.AddDefence);
            }
            else
            {
                Console.WriteLine("{0}. {1} : {4}$  |  체력 +{2}  |  공격력 +{3}  |  방어력 +{5}", i, E.Name, E.AddHealth, E.AddDamage, E.cost, E.AddDefence);
            }
        }
        public void Hospital(Player P)
        {
            Console.Clear();
            Console.WriteLine("현제 위치 : 병원\n\n");
            Console.WriteLine("병원에 들어왔습니다.\n");
            Console.WriteLine("{0}이(가) 가지고 있는 골드 : {1}$", P.Name, P.Gold);
            Console.WriteLine("{0}의 현제 체력 : {1} / {2}\n", P.Name, P.Health, P.MaxHealth + P.AddHealth);

            Console.WriteLine("상점 품목");

            Potion[] potion = new Potion[2]; //포션 종류 추가시 필수로 추가

            potion[0] = P.hpPotion;
            potion[1] = P.stPotion;
            int i = 1;

            foreach (Potion potions in potion) //포션에 대한 전체 설명
            {
                Console.WriteLine("{0}. {1} : {2}$  |  {3}",i,potions.Name,potions.cost,potions.explain);
                i++;
            }

            Console.WriteLine("{0} . 회복하기 : 300$  |  최대 체력의 50%를 회복합니다.",i++);
            Console.WriteLine("{0} . 돌아가기\n",i);

            Console.WriteLine("원하는 상품의 번호를 입력해 주세요.\n");
            int j = 0;
            while (true)
            {
                j = Input.Input(i); //1~i 숫자 입력 받기
                if (j != 0)
                {
                    break;
                }
            }
            if(j == i - 1) //50% 힐
            {
                if(P.Gold >= 300)
                {
                    Console.WriteLine("300$를 사용하여 최대 체력의 50%를 회복했습니다.");
                    P.TakeHeal((P.MaxHealth+P.AddHealth)/2);
                    P.Gold -= 300;
                }
                else
                {
                    Console.WriteLine("{0}은(는) 300$가 없습니다.", P.Name);
                }
                Console.WriteLine("아무키나 입력하세요.....");
                Console.ReadKey();
                Hospital(P); //병원에 계속 머물도록
            }
            else if(j != i) //아이템 구매
            {
                if(P.Gold >= potion[j - 1].cost)
                {
                    Console.WriteLine("{0}은(는) {1}$을 소모하였습니다.", P.Name, potion[j-1].cost);
                    potion[j - 1].Get();
                    P.Gold -= potion[j-1].cost;
                }
                else
                {
                    Console.WriteLine("{0}은(는) {1}$만큼의 골드가 없습니다.", P.Name, potion[j - 1].cost);
                }
                Console.WriteLine("아무키나 입력하세요.....");
                Console.ReadKey();
                Hospital(P); //병원에 계속 머물도록
            }
        }
    }

    public class Stage
    {
        public Player Player = new Player();
        public PlayerInput Input = new PlayerInput();
        public MakeUI MakeUI = new MakeUI();

        public void Start()
        {
            // 게임 시작 시 플레이어 명 입력
            Console.Write("플레이어의 이름을 입력해주세요 : ");
            Player.Name = Console.ReadLine();

            Console.WriteLine("\n플레이어의 이름은 {0}입니다.\n", Player.Name);
            Console.WriteLine("아무키나 입력하세요.....");
            Console.ReadKey();

            intro(); //인트로로
        }
        public void intro()
        {
            Console.Clear();
            Console.WriteLine("어서오세요 {0}님.\n", Player.Name);
            Console.WriteLine("이곳은 당신의 영지로 던전에 입장 전에 정비를 할 수 있습니다.\n");

            Console.WriteLine("아무키나 입력하세요.....");
            Console.ReadKey();

            Home();
        }
        public void Home()
        {
            int i;
            Console.Clear();

            Console.WriteLine("현제 위치 : 영지\n\n");
            Console.WriteLine("던전에 입장 전에 준비를 하세요.\n");
            Console.WriteLine("1. 상태 확인");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 무기 상점 이동");
            Console.WriteLine("4. 병원 이동");
            Console.WriteLine("5. 던전 이동\n");

            while (true)
            {
                i = Input.Input(5);
                if (i != 0)
                {
                    break;
                }
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine();
                    MakeUI.checkstate(Player);
                    Home(); //채크 끝날 시 다시 홈으로
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("인벤토리를 확인합니다.\n");
                    MakeUI.checkEquipinven(Player, false, false);
                    Console.WriteLine();
                    MakeUI.checkuseinven(Player, false, false);
                    Console.WriteLine();

                    Console.WriteLine("1. 아이템 착용");
                    Console.WriteLine("2. 아이템 설명");
                    Console.WriteLine("3. 돌아가기\n");

                    int j = 0;
                    while (true)
                    {
                        j = Input.Input(3); //1~3 숫자 입력 받기
                        if (j != 0)
                        {
                            break;
                        }
                    }

                    switch (j) //입력에 따른 상호작용
                    {
                        case 1:
                            Console.Clear();
                            MakeUI.checkEquipinven(Player, true, false);
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("아이템 설명을 확인합니다.\n");
                            MakeUI.checkEquipinven(Player, false, true);
                            Console.WriteLine();
                            MakeUI.checkuseinven(Player, false, true);

                            Console.WriteLine("\n아무키나 입력하세요.....");
                            Console.ReadKey();
                            break;
                    }
                    Home();
                    break;

                case 3: //무기 상점 이동
                    MakeUI.equipstore(Player);
                    Home();
                    break;

                case 4:
                    MakeUI.Hospital(Player);
                    Home();
                    break;

                case 5:
                    Console.Clear();
                    Console.Write("던전 입구로 이동 중입니다..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    ChooseDunguen();
                    break;
            }
        }
        public void ChooseDunguen()
        {
            int i = 0;
            Console.Clear();

            Console.WriteLine("현제 위치 : 던전 입구\n\n");
            Console.WriteLine("어느 던전에 입장할 지 선택하세요.\n");

            Console.WriteLine("1. 일반 던전(1레벨부터 입장가능)");
            Console.WriteLine("2. 중급 던전(3레벨부터 입장가능)");
            Console.WriteLine("3. 상급 던전(10레벨부터 입장가능)");
            Console.WriteLine("4. 마지막 던전");
            Console.WriteLine("5. 영지 이동\n");

            while (true)
            {
                i = Input.Input(5);
                if (i != 0)
                {
                    break;
                }
            }

            switch (i)
            {
                case 1:
                    Console.Clear();
                    Console.Write("일반 던전으로 이동 중입니다..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    NormalDunguen();
                    break;
                case 2:
                    if(Player.Level >= 3)
                    {
                        Console.Clear();
                        Console.Write("중급 던전으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        MiddleDunguen();
                    }
                    else
                    {
                        Console.WriteLine("레벨이 부족합니다.");
                        Console.WriteLine("\n아무키나 입력하세요.....");
                        Console.ReadKey();
                    }
                    break;
                case 3:
                    if (Player.Level >= 10)
                    {
                        Console.Clear();
                        Console.Write("상급 던전으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        HighDunguen();
                    }
                    else
                    {
                        Console.WriteLine("레벨이 부족합니다.");
                        Console.WriteLine("\n아무키나 입력하세요.....");
                        Console.ReadKey();
                    }
                    break;
                case 4:
                    Console.Clear();
                    Console.Write("마지막 던전으로 이동 중입니다..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    LastDunguen();
                    break;
                case 5:
                    Console.Clear();
                    Console.Write("영지으로 이동 중입니다..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Console.Write("..");
                    Thread.Sleep(1000);
                    Home();
                    break;
            }
        }
        public void NormalDunguen()
        {

        }
        public void MiddleDunguen()
        {

        }
        public void HighDunguen()
        {

        }
        public void LastDunguen()
        {

        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Stage stage = new Stage();
            stage.Start();
        }
    }
}