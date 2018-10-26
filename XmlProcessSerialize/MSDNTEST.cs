using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XmlProcessSerialize
{
    class MSDNTEST
    {
    }
    // XmlRootAttribute允许您设置备用名称（Purchase Order）表示XML元素及其名称空间。通过默认情况下，XmlSerializer使用类名。属性  
    //还允许您为元素设置XML命名空间。最后，该属性设置IsNullable属性，该属性指定是否如果类实例设置为，则会出现xsi：null属性
    //一个空引用。 
    [XmlRootAttribute("PurchaseOrder", Namespace = "http://www.cpandl.com",
    IsNullable = false)]
    public class PurchaseOrder
    {
        public Address ShipTo;
        public string OrderDate;
        // Xml数组属性更改XML元素名称从默认的“有序物品”到“物品”。 
        [XmlArrayAttribute("Items")]
        public OrderedItem[] OrderedItems;
        public decimal SubTotal;
        public decimal ShipCost;
        public decimal TotalCost;
    }

    public class Address
    {
        // Xml属性指示Xml Serializer序列化
        //将字段命名为XML属性而不是XML元素（
        //默认行为）。
        [XmlAttribute]
        public string Name;
        public string Line1;

        // 将Is Nullable属性设置为false指示
        // Xml Serializer，如果出现XML属性，则不会出现
        // City字段设置为空引用。
        [XmlElementAttribute(IsNullable = false)]
        public string City;
        public string State;
        public string Zip;
    }

    public class OrderedItem
    {
        public string ItemName;
        public string Description;
        public decimal UnitPrice;
        public int Quantity;
        public decimal LineTotal;

        // Calculate是一种计算每件商品价格的自定义方法
        //并将值存储在字段中。
        public void Calculate()
        {
            LineTotal = UnitPrice * Quantity;
        }
    }

    public class MsdnTest
    {
        public static void Main2()
        {
            // 读写订单。
            MsdnTest t = new MsdnTest();
            t.CreatePO("po.xml");
            t.ReadPO("po.xml");
        }

        public void CreatePO(string filename)
        {
            //创建Xml Serializer类的实例;
            //指定要序列化的对象的类型。
            XmlSerializer serializer =
            new XmlSerializer(typeof(PurchaseOrder));
            TextWriter writer = new StreamWriter(filename);
            PurchaseOrder po = new PurchaseOrder();

            //创建要发货和开票的地址。
            Address billAddress = new Address();
            billAddress.Name = "Teresa Atkinson";
            billAddress.Line1 = "1 Main St.";
            billAddress.City = "AnyTown";
            billAddress.State = "WA";
            billAddress.Zip = "00000";
            // 将Ship To和Bill To设置为同一个收件人。
            po.ShipTo = billAddress;
            po.OrderDate = System.DateTime.Now.ToLongDateString();

            //创建有序项。
            OrderedItem i1 = new OrderedItem();
            i1.ItemName = "Widget S";
            i1.Description = "Small widget";
            i1.UnitPrice = (decimal)5.23;
            i1.Quantity = 3;
            i1.Calculate();

            // 将项插入数组。
            OrderedItem[] items = { i1 };
            po.OrderedItems = items;
            //计算总成本。
            decimal subTotal = new decimal();
            foreach (OrderedItem oi in items)
            {
                subTotal += oi.LineTotal;
            }
            po.SubTotal = subTotal;
            po.ShipCost = (decimal)12.51;
            po.TotalCost = po.SubTotal + po.ShipCost;
            //序列化采购订单，并关闭文本编写器。
            serializer.Serialize(writer, po);
            writer.Close();
        }

        public void ReadPO(string filename)
        {
            // 创建Xml Serializer类的实例;
            //指定要反序列化的对象的类型。
            XmlSerializer serializer = new XmlSerializer(typeof(PurchaseOrder));
            // 如果XML文档已被更改为未知
            //节点或属性，用它来处理它们
            //未知节点和未知属性事件。
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            //读取XML文档需要文件流。
            FileStream fs = new FileStream(filename, FileMode.Open);
            //声明要反序列化的类型的对象变量。
            PurchaseOrder po;
            //使用Deserialize方法恢复对象的状态
            //使用XML文档中的数据。 * /
            po = (PurchaseOrder)serializer.Deserialize(fs);
            // Reads the order date.
            Console.WriteLine("OrderDate: " + po.OrderDate);

            // 阅读送货地址。
            Address shipTo = po.ShipTo;
            ReadAddress(shipTo, "Ship To:");
            // 读取订购商品列表。
            OrderedItem[] items = po.OrderedItems;
            Console.WriteLine("Items to be shipped:");
            foreach (OrderedItem oi in items)
            {
                Console.WriteLine("\t" +
                oi.ItemName + "\t" +
                oi.Description + "\t" +
                oi.UnitPrice + "\t" +
                oi.Quantity + "\t" +
                oi.LineTotal);
            }
            //读取小计，运费和总费用。
            Console.WriteLine(
            "\n\t\t\t\t\t Subtotal\t" + po.SubTotal +
            "\n\t\t\t\t\t Shipping\t" + po.ShipCost +
            "\n\t\t\t\t\t Total\t\t" + po.TotalCost
            );
        }

        public void ReadAddress(Address a, string label)
        {
            //读取地址的字段。
            Console.WriteLine(label);
            Console.Write("\t" +
            a.Name + "\n\t" +
            a.Line1 + "\n\t" +
            a.City + "\t" +
            a.State + "\n\t" +
            a.Zip + "\n");
        }

        public void serializer_UnknownNode
        (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        public void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}
