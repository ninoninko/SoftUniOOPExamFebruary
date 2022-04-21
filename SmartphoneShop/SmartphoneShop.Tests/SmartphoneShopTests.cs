using NUnit.Framework;
using System;

namespace SmartphoneShop.Tests
{
    [TestFixture]
    public class SmartphoneShopTests
    {
       [Test]
       public void testCapacity()
       {
            Shop shop = new Shop(5);
            Assert.IsTrue(shop.Capacity == 5);
       }

        [Test]
        public void testCapacityException()
        {
            Shop shop = null;
            Assert.Throws<ArgumentException>(() => shop = new Shop(-2));
        }

        [Test]
        public void testCountOne()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            Assert.IsTrue(shop.Count == 1);
        }

        [Test]
        public void testCountMultiple()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            Smartphone phone1 = new Smartphone("sama", 50);
            Smartphone phone2 = new Smartphone("samu", 50);
            Smartphone phone3 = new Smartphone("same", 50);
            shop.Add(phone);
            shop.Add(phone1);
            shop.Add(phone2);
            shop.Add(phone3);
            Assert.IsTrue(shop.Count == 4);
        }


        [Test]
        public void testAddButExisting()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            Smartphone phone1 = new Smartphone("sama", 50);
            Smartphone phone2 = new Smartphone("samu", 50);
            Smartphone phone3 = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.Add(phone1);
            shop.Add(phone2);            
            Assert.Throws<InvalidOperationException>(() => shop.Add(phone3));          
        }

        [Test]
        public void testAddButFull()
        {
            Shop shop = new Shop(3);
            Smartphone phone = new Smartphone("sam", 50);
            Smartphone phone1 = new Smartphone("sama", 50);
            Smartphone phone2 = new Smartphone("samu", 50);
            Smartphone phone3 = new Smartphone("same", 50);
            shop.Add(phone);
            shop.Add(phone1);
            shop.Add(phone2);
            Assert.Throws<InvalidOperationException>(() => shop.Add(phone3));
        }

        [Test]
        public void testRemoveOne()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.Remove("sam");
            Assert.IsTrue(shop.Count == 0);
        }

        [Test]
        public void testRemoveMultiple()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            Smartphone phone1 = new Smartphone("sama", 50);
            Smartphone phone2 = new Smartphone("samu", 50);
            Smartphone phone3 = new Smartphone("same", 50);
            shop.Add(phone);
            shop.Add(phone1);
            shop.Add(phone2);
            shop.Add(phone3);
            shop.Remove("sama");
            shop.Remove("same");
            shop.Remove("sam");
            Assert.IsTrue(shop.Count == 1);
        }

        [Test]
        public void testRemoveNonExisting()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            Smartphone phone1 = new Smartphone("sama", 50);
            Smartphone phone2 = new Smartphone("samu", 50);
            Smartphone phone3 = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.Add(phone1);
            shop.Add(phone2);
            Assert.Throws<InvalidOperationException>(() => shop.Remove("same"));
        }

        [Test]
        public void testBattery()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.TestPhone("sam", 40);
            Assert.IsTrue(phone.CurrentBateryCharge == 10);
        }

        [Test]
        public void testBatteryPhoneNonExistent()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            
            Assert.Throws<InvalidOperationException>(() => shop.TestPhone("same", 40));
        }

        [Test]
        public void testBatteryPhoneLowBattery()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);

            Assert.Throws<InvalidOperationException>(() => shop.TestPhone("sam", 60));
        }


        [Test]
        public void testChargePhone()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.TestPhone("sam", 40);
            shop.ChargePhone("sam");
            Assert.IsTrue(phone.CurrentBateryCharge == 50);
        }

        [Test]
        public void testChargePhoneWrong()
        {
            Shop shop = new Shop(5);
            Smartphone phone = new Smartphone("sam", 50);
            shop.Add(phone);
            shop.TestPhone("sam", 40);
            Assert.Throws<InvalidOperationException>(() => shop.ChargePhone("same"));           
        }
    }
}