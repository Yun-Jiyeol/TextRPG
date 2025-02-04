using System;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
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

            Gold = 6000;

            Attack = 10;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 5;
            itemDefence = 0;

            IsDead = false;
        }
    }
    public class Monster : ICharacter { }

    public class Goblin : Monster
    {
        public Goblin()
        {
            Name = "고블린 일꾼";
            job = "일꾼";

            Level = 0;
            Ex = 10;
            MaxEx = 0;

            Health = 30;
            MaxHealth = 30;

            Gold = -1;

            Attack = 15;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 0;
            itemDefence = 0;

            IsDead = false;
        }
    }
    public class GoblinChecker : Monster
    {
        public GoblinChecker()
        {
            Name = "고블린 정찰병";
            job = "정찰병";

            Level = 0;
            Ex = 30;
            MaxEx = 0;

            Health = 80;
            MaxHealth = 80;

            Gold = -1;

            Attack = 30;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 3;
            itemDefence = 0;

            IsDead = false;
        }
    }
    public class GoblinWarrior : Monster
    {
        public GoblinWarrior()
        {
            Name = "고블린 전사";
            job = "전사";

            Level = 0;
            Ex = 70;
            MaxEx = 0;

            Health = 150;
            MaxHealth = 150;

            Gold = -1;

            Attack = 45;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 5;
            itemDefence = 0;

            IsDead = false;
        }
    }
    public class GoblinKing : Monster
    {
        public GoblinKing()
        {
            Name = "고블린 왕";
            job = "**최종보스**";

            Level = 0;
            Ex = 0;
            MaxEx = 0;

            Health = 300;
            MaxHealth = 300;

            Gold = -1;

            Attack = 10;
            AddAttack = 0;
            itemAttack = 0;

            Defence = 50;
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
        Random random = new Random();

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
        public int[] RandomRoom(int type)
        {
            if (type == 3)
            {
                int[] arr = { 0, 1, 1, 2, 2, 2, 2, 3, 3 }; //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방
                RandomArray(arr);
                return arr;
            }
            else if (type == 4)
            {
                int[] arr = { 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 }; //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방
                RandomArray(arr);
                return arr;
            }
            else if (type == 5)
            {
                int[] arr = { 0,0,0,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3}; //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방
                RandomArray(arr);
                return arr;
            }
            else{
                int[] arr = { 0, 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3 }; //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방
                RandomArray(arr);
                return arr;
            }
        }
        public int[] RandomArray(int[] arr)
        {
            Random random = new Random();
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int j = random.Next(i, arr.Length - 1);
                int temp = arr[j];
                arr[j] = arr[i];
                arr[i] = temp;
            }
            return arr;
        }
        public void MakeDuguen(int size , int[] room)
        {
            int x;
            int y = 0;

            for (int repeat =0; repeat < size* size; repeat++)
            {
                x = (repeat % size) * 5;
                y = (repeat / size) * 2;

                for (int i = 0; i < 5; i++) //x축 맵 그림
                {
                    Console.SetCursorPosition(i + x, y);
                    Console.Write('*');
                    Console.SetCursorPosition(i + x, 2 + y);
                    Console.Write('*');
                }
                for (int i = 0; i < 2; i++) //y축 맵 그림
                {
                    Console.SetCursorPosition(x, i + y);
                    Console.Write('*');
                    Console.SetCursorPosition(5 + x, i + y);
                    Console.Write('*');
                }
                Console.SetCursorPosition(2 + x, 1 + y);
                Console.Write(room[repeat]);
            }

            Console.SetCursorPosition(0, y + 3);
        }
        public Monster RandomMonster(int size)
        {
            int rannum = random.Next(1, 101); //1~100까지 랜덤 정수 받기
            if (size == 3) //일반 던전
            {
                if (rannum <= 80) //80프로 확률로 고블린, 20프로 확률 정찰병
                {
                    Goblin goblin = new Goblin();
                    return goblin;
                }
                else
                {
                    GoblinChecker goblin = new GoblinChecker();
                    return goblin;
                }
            }else if(size == 4)
            {
                if (rannum <= 50) //50프로 확률로 고블린, 40프로 확률 정찰병, 10프로 확률 전사
                {
                    Goblin goblin = new Goblin();
                    return goblin;
                }
                else if(rannum <= 90)
                {
                    GoblinChecker goblin = new GoblinChecker();
                    return goblin;
                }
                else
                {
                    GoblinWarrior goblin = new GoblinWarrior();
                    return goblin;
                }
            }
            else if (size == 5)
            {
                if (rannum <= 20) //20프로 확률로 고블린, 50프로 확률 정찰병, 30프로 확률 전사
                {
                    Goblin goblin = new Goblin();
                    return goblin;
                }
                else if (rannum <= 70)
                {
                    GoblinChecker goblin = new GoblinChecker();
                    return goblin;
                }
                else
                {
                    GoblinWarrior goblin = new GoblinWarrior();
                    return goblin;
                }
            }
            else //마지막 던전용
            {
                if (rannum <= 60)
                {
                    GoblinChecker goblin = new GoblinChecker();
                    return goblin;
                }
                else
                {
                    GoblinWarrior goblin = new GoblinWarrior();
                    return goblin;
                }
            }
        }

        public void PlayerGetEx(Player P, Monster M)
        {
            P.Ex += M.Ex;
            if(P.Ex > P.MaxEx)
            {
                Console.WriteLine("*****래벨 업*****");
                P.Level += 1; //player 랩업
                P.Ex -= P.MaxEx;
                P.MaxEx += P.MaxEx/2; //최대 경험치 증가

                P.MaxHealth += 10; //스텟 증가
                P.Attack += 2;
                P.Defence += 1;
                P.TakeHeal(50); //랩업에 따른 회복
            }
        }

        public void GoldRoom(Player P , int min , int max) //보물방용
        {
            int getgold = random.Next(min,max);
            Console.WriteLine("{0}은(는) {1}의 골드를 획득했습니다!\n",P.Name,getgold);
            P.Gold += getgold;

            int randomdrop = random.Next(1,101);
            if(randomdrop > 50)
            {
                Console.WriteLine("{0}은(는) {1}(을)를 찾았습니다!", P.Name, P.hpPotion.Name);
                P.hpPotion.Get();
            }else if(randomdrop > 80)
            {
                Console.WriteLine("{0}은(는) {1}(을)를 찾았습니다!", P.Name, P.stPotion.Name);
                P.stPotion.Get();
            }
            else
            {
                Console.WriteLine("다른건 없었습니다.");
            }
        }

        public void DropTable(Player P, Monster M) //몬스터를 잡을 시
        {
            int max = 0, min = 0;
            switch (M.Name)
            {
                case "고블린 일꾼":
                    min = 0;
                    max = 100;
                    break;
                case "고블린 정찰병":
                    min = 40;
                    max = 140;
                    break;
                case "고블린 전사":
                    min = 80;
                    max = 180;
                    break;
                case "고블린 왕":
                    Console.WriteLine("{0}의 노력 끝에 고블린 킹은 사망하였습니다!");
                    Console.WriteLine("고블린 킹이 없어진 마을은 안전해졌습니다.\n");
                    Console.WriteLine("********CONGRATURATION********\n\n");
                    string filePath = "SavePlayer.json";
                    File.Delete(filePath); //클리어 시 게임 초기화
                    Environment.Exit(0);
                    break;
            }

            int randomdrop = random.Next(0, max);
            if (randomdrop > 60)
            {
                Console.WriteLine("{0}은(는) {1}(을)를 찾았습니다!", P.Name, P.hpPotion.Name);
                P.hpPotion.Get();
            }
            else if (randomdrop > 120)
            {
                Console.WriteLine("{0}은(는) {1}(을)를 찾았습니다!", P.Name, P.stPotion.Name);
                P.stPotion.Get();
            }
            else
            {
                Console.WriteLine("다른건 없었습니다.");
            }
        }
    }
    public class MakeUI
    {
        public PlayerInput Input = new PlayerInput();
        public void checkstate(ICharacter C)
        {
            Console.WriteLine("이름 : {0}", C.Name);
            Console.WriteLine("직업 : {0}", C.job);
            if(C.Level > 0)
            {
                Console.WriteLine("레벨 : {0}", C.Level);
                Console.WriteLine("경험치 : {0} / {1}", C.Ex, C.MaxEx);
            }
            else
            {
                Console.WriteLine("주는 경험치 : {0}", C.Ex);
            }

            if(C.Gold >= 0)
            {
                Console.WriteLine("소유 골드 : {0}$", C.Gold);
            }

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
            int num = 0, numsword = 0, numsuit = 0;

            if (P.normalsord.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.normalsord;
                numequip[num] = num + 1;
                num++;
                numsword++;
            }
            if (P.IronSword.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.IronSword;
                numequip[num] = num + 1;
                num++;
                numsword++;
            }
            if (P.normalSuit.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.normalSuit;
                numequip[num] = num + 1;
                num++;
                numsuit++;
            }
            if (P.ironSuit.PlayerHave) //무기 획득 유무
            {
                Phave[num] = P.ironSuit;
                numequip[num] = num + 1;
                num++;
                numsuit++;
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
                    if(j <= numsword) //같은 느낌의 장비는 해제 | 하지만 해제는 불가능
                    {
                        for(int x = 0; x <  numsword; x++)
                        {
                            if (Phave[x].PlayerUse)
                            {
                                Phave[x].Use(P);
                            }
                        }
                    }else if(j <= numsword + numsuit)
                    {
                        for (int x = numsword; x < numsword + numsuit; x++)
                        {
                            if (Phave[x].PlayerUse)
                            {
                                Phave[x].Use(P);
                            }
                        }
                    }
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
                Console.WriteLine("{0}의 현제 체력 : {1} / {2}\n", P.Name,P.Health,P.AddHealth+P.MaxHealth);
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
        public void MvsP(Monster M, Player P)
        {
            Console.WriteLine("\t{0} : {1} / {2}\n", M.Name, M.Health, M.MaxHealth);
            Console.WriteLine("\t{0} : {1} / {2}\n", P.Name, P.Health, P.MaxHealth + P.AddHealth);

            Console.WriteLine("1. 공격");
            Console.WriteLine("2. 상태 확인");
            Console.WriteLine("3. 아이템 사용\n");

            int i = 0;
            while (true)
            {
                i = Input.Input(3);
                if (i != 0)
                {
                    break;
                }
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine("당신은 공격을 선택하였습니다.\n");
                    Console.WriteLine("{0}의 공격으로 {1}은(는) {2}만큼의 데미지를 입었습니다.\n\n", P.Name, M.Name, P.Attack + P.AddAttack + P.itemAttack - M.Defence);
                    M.TakeDamage(P.Attack + P.AddAttack + P.itemAttack);

                    Thread.Sleep(1000); //잠시 텀을 준다

                    if (M.IsDead) //몬스터의 사망확인
                    {
                        Input.DropTable(P,M);
                        Input.PlayerGetEx(P,M);
                        P.AddAttack = 0; //포션 초기화
                        Console.WriteLine("{0}을 잡고 {1}은 던전으로 귀환했습니다.\n", M.Name, P.Name);
                    }
                    else
                    {
                        Console.WriteLine("{0}이 {1}을(를) 공격했습니다.\n", M.Name, P.Name);
                        int a = 0;
                        if ((M.Attack - P.Defence - P.itemDefence) > 0)
                        {
                            a = (M.Attack - P.Defence - P.itemDefence);
                        }
                        Console.WriteLine("{0}의 공격으로 {1}은(는) {2}만큼의 데미지를 입었습니다.\n", M.Name, P.Name,a);
                        P.TakeDamage(M.Attack);

                        if (P.IsDead) //플레이어의 사망 확인
                        {
                            Console.WriteLine("{0}이(가) 사망하였습니다...\n", M.Name);
                            Console.WriteLine("-The End-\n");
                            string filePath = "SavePlayer.json";
                            File.Delete(filePath); //사망 시 초기화
                            Environment.Exit(0);
                        }
                    }
                    Thread.Sleep(1000); //잠시 텀을 준다
                    break;

                case 2:
                    Console.WriteLine("당신은 상태 확인을 선택했습니다.\n");
                    checkstate(P);
                    Console.WriteLine();
                    checkstate(M);
                    Console.WriteLine();
                    break;

                case 3:
                    Console.WriteLine("당신은 아이템 사용을 선택하였습니다.\n");
                    checkuseinven(P, true, false);
                    break;
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

            Home(); //홈으로
        }
        public void intro()
        {
            string filePath = "SavePlayer.json"; //저장된 내용 가져오기

            string[] lines = File.ReadAllLines(filePath);
            Player = JsonSerializer.Deserialize<Player>(lines[0]);
            Player.normalsord = JsonSerializer.Deserialize<NormalSword>(lines[1]);
            Player.IronSword = JsonSerializer.Deserialize<IronSword>(lines[2]);
            Player.normalSuit = JsonSerializer.Deserialize<NormalSuit>(lines[3]);
            Player.ironSuit = JsonSerializer.Deserialize<IronSuit>(lines[4]);
            Player.hpPotion = JsonSerializer.Deserialize<HpPotion>(lines[5]);
            Player.stPotion = JsonSerializer.Deserialize<StPotion>(lines[6]);

            Console.Clear();
            Console.WriteLine("어서오세요 {0}님.\n", Player.Name);
            Console.WriteLine("이곳은 당신의 영지로 던전에 입장 전에 정비를 할 수 있습니다.\n");

            Console.WriteLine("아무키나 입력하세요.....");
            Console.ReadKey();

            Home();
        }
        public void Home()
        {
            //마을에 들어올 때만 저장
            string filePath = "SavePlayer.json"; // 플레이어 현 상태 저장 + 장비, 소모품의 획득량 저장
            string player = JsonSerializer.Serialize(Player);
            string normalsword = JsonSerializer.Serialize(Player.normalsord);
            string ironsword = JsonSerializer.Serialize(Player.IronSword);
            string normalsuit = JsonSerializer.Serialize(Player.normalSuit);
            string ironsuit = JsonSerializer.Serialize(Player.ironSuit);
            string hpposion = JsonSerializer.Serialize(Player.hpPotion);
            string stposion = JsonSerializer.Serialize(Player.stPotion);
            string[] json = {player,normalsword,ironsword,normalsuit,ironsuit,hpposion,stposion };
            
            File.WriteAllLines(filePath, json);

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
                        ChooseDunguen();
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
                        ChooseDunguen();
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
            Console.Clear();
            int size = 3;
            int floor = 1;
            int maxfloor = 2;
            int[] room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
            //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
            int[] makeroom = { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; //방을 번호로
            Input.MakeDuguen(size, makeroom);

            Console.WriteLine("\n10. 도주\n");
            Console.WriteLine("현제 위치 : 일반 던전 {0}층", floor);
            Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
            int i = 0;
            while (true)
            {
                i = Input.Input(10);
                if (i != 0)
                {
                    if (i == 10)
                    {
                        Console.WriteLine("도주하기로 선택했습니다.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.Write("영지으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Home();
                    }
                    else
                    {
                        if (room[i-1] != 4)
                        {
                            Console.Clear();
                            Console.Write("{0}은(는) {1}번 째 방을 살펴보기로 했다\n", Player.Name, i);
                            Thread.Sleep(1000);
                            switch(room[i - 1])
                            {
                                case 0:
                                    if(floor == maxfloor)
                                    {
                                        Console.WriteLine("탈출구를 찾았습니다. 탈출하겠습니까?\n");
                                        Console.WriteLine("1. 탈출한다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("다음 층으로 가는 길을 찾았습니다. 가시겠습니까? (현제 층 : {0}, 마지막 층 : {1})\n", floor, maxfloor);
                                        Console.WriteLine("1. 다음 층으로 간다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    int j = 0; //선택
                                    while (true)
                                    {
                                        j = Input.Input(2);
                                        if (j != 0)
                                        {
                                            break;
                                        }
                                    }
                                    if (j == 1 && floor == maxfloor)
                                    {
                                        Console.Write("탈출하여 영지로 복귀 합니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        Home();
                                    }
                                    else if (j == 1 && floor != maxfloor) //막층
                                    {
                                        Console.Write("다음 층으로 이동중입니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        floor++; //다음층
                                        //방 초기화
                                        room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
                                        //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
                                        for(int e = 1; e <= size * size; e++)
                                        {
                                            makeroom[e - 1] = e;
                                        }
                                    }
                                    break;
                                case 1:
                                    Console.WriteLine("보물 방을 찾았습니다!\n");
                                    Console.WriteLine("보상을 획득 했습니다.\n");
                                    Input.GoldRoom(Player,50,150); //보물방 보상
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.WriteLine("몬스터가 기습했습니다!\n");
                                    Monster mon = Input.RandomMonster(size);
                                    while (true)
                                    {
                                        MakeUI.MvsP(mon, Player);
                                        if (mon.IsDead)
                                        {
                                            break;
                                        }
                                        Console.Clear();
                                    }
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.WriteLine("빈 방을 찾았습니다!\n");
                                    Console.WriteLine("다시 원래 장소로 돌아갑니다.\n");
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n{0}번 째 방은 이미 살펴본 방입니다.",i);
                            Console.WriteLine("아무키나 입력하세요.....");
                            Console.ReadKey();
                        }
                    }

                    Console.Clear();
                    Input.MakeDuguen(size, makeroom);
                    Console.WriteLine("\n10. 도주\n");
                    Console.WriteLine("현제 위치 : 일반 던전 {0}층", floor);
                    Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
                }
            }
        }
        public void MiddleDunguen()
        {
            Console.Clear();
            int size = 4;
            int floor = 1;
            int maxfloor = 3;
            int[] room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
            //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
            int[] makeroom = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }; //방을 번호로
            Input.MakeDuguen(size, makeroom);

            Console.WriteLine("\n17. 도주\n");
            Console.WriteLine("현제 위치 : 중급 던전 {0}층", floor);
            Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
            int i = 0;
            while (true)
            {
                i = Input.Input(17);
                if (i != 0)
                {
                    if (i == 17)
                    {
                        Console.WriteLine("도주하기로 선택했습니다.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.Write("영지으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Home();
                    }
                    else
                    {
                        if (room[i - 1] != 4)
                        {
                            Console.Clear();
                            Console.Write("{0}은(는) {1}번 째 방을 살펴보기로 했다\n", Player.Name, i);
                            Thread.Sleep(1000);
                            switch (room[i - 1])
                            {
                                case 0:
                                    if (floor == maxfloor)
                                    {
                                        Console.WriteLine("탈출구를 찾았습니다. 탈출하겠습니까?\n");
                                        Console.WriteLine("1. 탈출한다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("다음 층으로 가는 길을 찾았습니다. 가시겠습니까? (현제 층 : {0}, 마지막 층 : {1})\n", floor, maxfloor);
                                        Console.WriteLine("1. 다음 층으로 간다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    int j = 0; //선택
                                    while (true)
                                    {
                                        j = Input.Input(2);
                                        if (j != 0)
                                        {
                                            break;
                                        }
                                    }
                                    if (j == 1 && floor == maxfloor)
                                    {
                                        Console.Write("탈출하여 영지로 복귀 합니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        Home();
                                    }
                                    else if (j == 1 && floor != maxfloor) //막층
                                    {
                                        Console.Write("다음 층으로 이동중입니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        floor++; //다음층
                                        //방 초기화
                                        room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
                                        //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
                                        for (int e = 1; e <= size * size; e++)
                                        {
                                            makeroom[e - 1] = e;
                                        }
                                    }
                                    break;
                                case 1:
                                    Console.WriteLine("보물 방을 찾았습니다!\n");
                                    Console.WriteLine("보상을 획득 했습니다.\n");
                                    Input.GoldRoom(Player, 100, 250); //보물방 보상
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.WriteLine("몬스터가 기습했습니다!\n");
                                    Monster mon = Input.RandomMonster(size);
                                    while (true)
                                    {
                                        MakeUI.MvsP(mon, Player);
                                        if (mon.IsDead)
                                        {
                                            break;
                                        }
                                        Console.Clear();
                                    }
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.WriteLine("빈 방을 찾았습니다!\n");
                                    Console.WriteLine("다시 원래 장소로 돌아갑니다.\n");
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n{0}번 째 방은 이미 살펴본 방입니다.", i);
                            Console.WriteLine("아무키나 입력하세요.....");
                            Console.ReadKey();
                        }
                    }

                    Console.Clear();
                    Input.MakeDuguen(size, makeroom);
                    Console.WriteLine("\n17. 도주\n");
                    Console.WriteLine("현제 위치 : 중급 던전 {0}층", floor);
                    Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
                }
            }
        }
        public void HighDunguen()
        {
            Console.Clear();
            int size = 5;
            int floor = 1;
            int maxfloor = 3;
            int[] room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
            //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
            int[] makeroom = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,17,18,19,20,21,22,23,24,25}; //방을 번호로
            Input.MakeDuguen(size, makeroom);

            Console.WriteLine("\n26. 도주\n");
            Console.WriteLine("현제 위치 : 상급 던전 {0}층", floor);
            Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
            int i = 0;
            while (true)
            {
                i = Input.Input(26);
                if (i != 0)
                {
                    if (i == 26)
                    {
                        Console.WriteLine("도주하기로 선택했습니다.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.Write("영지으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Home();
                    }
                    else
                    {
                        if (room[i - 1] != 4)
                        {
                            Console.Clear();
                            Console.Write("{0}은(는) {1}번 째 방을 살펴보기로 했다\n", Player.Name, i);
                            Thread.Sleep(1000);
                            switch (room[i - 1])
                            {
                                case 0:
                                    if (floor == maxfloor)
                                    {
                                        Console.WriteLine("탈출구를 찾았습니다. 탈출하겠습니까?\n");
                                        Console.WriteLine("1. 탈출한다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("다음 층으로 가는 길을 찾았습니다. 가시겠습니까? (현제 층 : {0}, 마지막 층 : {1})\n", floor, maxfloor);
                                        Console.WriteLine("1. 다음 층으로 간다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    int j = 0; //선택
                                    while (true)
                                    {
                                        j = Input.Input(2);
                                        if (j != 0)
                                        {
                                            break;
                                        }
                                    }
                                    if (j == 1 && floor == maxfloor)
                                    {
                                        Console.Write("탈출하여 영지로 복귀 합니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        Home();
                                    }
                                    else if (j == 1 && floor != maxfloor) //막층
                                    {
                                        Console.Write("다음 층으로 이동중입니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        floor++; //다음층
                                        //방 초기화
                                        room = Input.RandomRoom(size); //무슨방인지 판단용(보이면 안됨)
                                        //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
                                        for (int e = 1; e <= size * size; e++)
                                        {
                                            makeroom[e - 1] = e;
                                        }
                                    }
                                    break;
                                case 1:
                                    Console.WriteLine("보물 방을 찾았습니다!\n");
                                    Console.WriteLine("보상을 획득 했습니다.\n");
                                    Input.GoldRoom(Player, 250, 500); //보물방 보상
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.WriteLine("몬스터가 기습했습니다!\n");
                                    Monster mon = Input.RandomMonster(size);
                                    while (true)
                                    {
                                        MakeUI.MvsP(mon, Player);
                                        if (mon.IsDead)
                                        {
                                            break;
                                        }
                                        Console.Clear();
                                    }
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.WriteLine("빈 방을 찾았습니다!\n");
                                    Console.WriteLine("다시 원래 장소로 돌아갑니다.\n");
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n{0}번 째 방은 이미 살펴본 방입니다.", i);
                            Console.WriteLine("아무키나 입력하세요.....");
                            Console.ReadKey();
                        }
                    }

                    Console.Clear();
                    Input.MakeDuguen(size, makeroom);
                    Console.WriteLine("\n26. 도주\n");
                    Console.WriteLine("현제 위치 : 상급 던전 {0}층", floor);
                    Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
                }
            }
        }
        public void LastDunguen()
        {
            Console.Clear();
            int size = 5;
            int floor = 1;
            int maxfloor = 4;
            int[] room = Input.RandomRoom(6); //무슨방인지 판단용(보이면 안됨)
            //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
            int[] makeroom = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 }; //방을 번호로
            Input.MakeDuguen(size, makeroom);

            Console.WriteLine("\n26. 도주\n");
            Console.WriteLine("현제 위치 : 마지막 던전 {0}층", floor);
            Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
            int i = 0;
            while (true)
            {
                i = Input.Input(26);
                if (i != 0)
                {
                    if (i == 26)
                    {
                        Console.WriteLine("도주하기로 선택했습니다.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.Write("영지으로 이동 중입니다..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Console.Write("..");
                        Thread.Sleep(1000);
                        Home();
                    }
                    else
                    {
                        if (room[i - 1] != 4)
                        {
                            Console.Clear();
                            Console.Write("{0}은(는) {1}번 째 방을 살펴보기로 했다\n", Player.Name, i);
                            Thread.Sleep(1000);
                            switch (room[i - 1])
                            {
                                case 0:
                                    if (floor == maxfloor)
                                    {
                                        Console.WriteLine("탈출구를 찾았습니다. 탈출하겠습니까?\n");
                                        Console.WriteLine("1. 탈출한다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("다음 층으로 가는 길을 찾았습니다. 가시겠습니까? (현제 층 : {0}, 마지막 층 : {1})\n", floor, maxfloor);
                                        Console.WriteLine("1. 다음 층으로 간다.");
                                        Console.WriteLine("2. 이번 층을 더 둘러본다.\n");
                                    }
                                    int j = 0; //선택
                                    while (true)
                                    {
                                        j = Input.Input(2);
                                        if (j != 0)
                                        {
                                            break;
                                        }
                                    }
                                    if (j == 1 && floor == maxfloor) //보스 기습
                                    {
                                        Console.WriteLine("보스가 기습했습니다!\n");
                                        GoblinKing boss = new GoblinKing();
                                        while (true)
                                        {
                                            MakeUI.MvsP(boss, Player);
                                            if (boss.IsDead)
                                            {
                                                break;
                                            }
                                            Console.Clear();
                                        }
                                    }
                                    else if (j == 1 && floor != maxfloor) //막층
                                    {
                                        Console.Write("다음 층으로 이동중입니다..");
                                        Thread.Sleep(1000);
                                        Console.Write("..");
                                        Thread.Sleep(1000);
                                        floor++; //다음층
                                        //방 초기화
                                        room = Input.RandomRoom(6); //무슨방인지 판단용(보이면 안됨)
                                        //0= 다음 층 또는 클리어 | 1 = 보물방 | 2 = 몬스터 | 3 = 빈방 | 4 = 이미 확인한 방
                                        for (int e = 1; e <= size * size; e++)
                                        {
                                            makeroom[e - 1] = e;
                                        }
                                    }
                                    break;
                                case 1:
                                    Console.WriteLine("보물 방을 찾았습니다!\n");
                                    Console.WriteLine("보상을 획득 했습니다.\n");
                                    Input.GoldRoom(Player, 250, 500); //보물방 보상
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 2:
                                    Console.WriteLine("몬스터가 기습했습니다!\n");
                                    Monster mon = Input.RandomMonster(size);
                                    while (true)
                                    {
                                        MakeUI.MvsP(mon, Player);
                                        if (mon.IsDead)
                                        {
                                            break;
                                        }
                                        Console.Clear();
                                    }
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                                case 3:
                                    Console.WriteLine("빈 방을 찾았습니다!\n");
                                    Console.WriteLine("다시 원래 장소로 돌아갑니다.\n");
                                    makeroom[i - 1] = 0;
                                    room[i - 1] = 4;
                                    Console.WriteLine("\n아무키나 입력하세요.....");
                                    Console.ReadKey();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\n{0}번 째 방은 이미 살펴본 방입니다.", i);
                            Console.WriteLine("아무키나 입력하세요.....");
                            Console.ReadKey();
                        }
                    }

                    Console.Clear();
                    Input.MakeDuguen(size, makeroom);
                    Console.WriteLine("\n26. 도주\n");
                    Console.WriteLine("현제 위치 : 상급 던전 {0}층", floor);
                    Console.WriteLine("원하는 방 번호 또는 도주를 선택해 주세요");
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Stage stage = new Stage();
            string filePath = "SavePlayer.json";

            if (File.Exists(filePath)) //저장된 플레이어 정보가 존재한다면
            {
                stage.intro();
            }
            else
            {
                stage.Start();
            }
        }
    }
}