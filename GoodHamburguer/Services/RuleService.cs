using GoodHamburguer.Models;
using System.Collections.Generic;

namespace GoodHamburguer.Services
{
    public class RuleService
    {
        private readonly SandwichModel sandwichModel;
        private readonly ExtraModel extraModel;
        private  OrderListModel _orderListModel;

        public RuleService() {
            // Initializing the available sandwiches and extras
            List<string> sandwiches = new List<string> { "X Burger", "X Egg", "X Bacon" };
            List<string> extras = new List<string> { "Fries", "Soft drink"};

            // Creating instances of SandwichModel, ExtraModel, and OrderListModel
            sandwichModel = new SandwichModel
            {
                Sandwiches = sandwiches,
            };

            extraModel = new ExtraModel
            {
                Extras = extras,
            };
            _orderListModel = new OrderListModel
            {
                OrderItems = new List<OrderItemFullModel>()
            };
        }

        // Method to retrieve a dictionary containing lists of sandwiches and extras
        public Dictionary<string, List<string>> ListAll()
        {
            var listSandwiches = ListSandwiches() ?? new List<string>();
            var listExtas = ListExtras() ?? new List<string>();
            
            var result = new Dictionary<string, List<string>>
        {
            { "Sandwiches", listSandwiches },
            { "Extas", listExtas }
        };
            return result;
        }

        // Method to retrieve the list of sandwiches
        public List<string>? ListSandwiches()
        {
            return sandwichModel.Sandwiches;
        }

        // Method to retrieve the list of extras
        public List<string>? ListExtras()
        {
            return extraModel.Extras;
        }

        // Method to calculate the price of an order and apply discounts
        public OrderItemFullModel AmountCharged(OrderItemModel item)
        {
            double xburgerPrice = 5.00;
            double xEggPrice = 4.50;
            double xBaconPrice = 7.00;
            double friesPrice = 2.00;
            double softDrinkPrice = 2.50;

            OrderItemFullModel orderItemFull = new OrderItemFullModel
            {
                Sandwich = item.Sandwich,
                Extra = item.Extra,
                Price = 0 
            };

            if (HasMultipleSelectionsFromSameCategory(orderItemFull.Sandwich, orderItemFull.Extra))
            {
                throw new InvalidOperationException("Não selecione mais de uma opção da mesma categoria (sanduíche, fritas ou bebida).");
            }

            if (orderItemFull.Sandwich != null)
            {
                switch (orderItemFull.Sandwich.ToLower())
                {
                    case "x burger":
                        orderItemFull.Price += xburgerPrice;
                        break;
                    case "x egg":
                        orderItemFull.Price += xEggPrice;
                        break;
                    case "x bacon":
                        orderItemFull.Price += xBaconPrice;
                        break;
                    default:
                        break;
                }
            }

                if (orderItemFull.Extra != null)
                {
                    foreach (var extra in orderItemFull.Extra)
                    {
                        switch (extra.ToLower())
                        {
                            case "fries":
                                orderItemFull.Price += friesPrice;
                                break;
                            case "soft drink":
                                orderItemFull.Price += softDrinkPrice;
                                break;
                            default:
                                break;
                        }
                    }
                }

             AppyDiscont(orderItemFull);
            _orderListModel.OrderItems.Add(orderItemFull);

            return orderItemFull;
        }

        // Method to update the price of an existing order
        public OrderItemFullModel AmountCharged(OrderItemFullModel item)
        {
            item.Price = 0;
            double xburgerPrice = 5.00;
            double xEggPrice = 4.50;
            double xBaconPrice = 7.00;
            double friesPrice = 2.00;
            double softDrinkPrice = 2.50;



            if (item.Sandwich != null)
            {
                switch (item.Sandwich.ToLower())
                {
                    case "x burger":
                        item.Price += xburgerPrice;
                        break;
                    case "x egg":
                        item.Price += xEggPrice;
                        break;
                    case "x bacon":
                        item.Price += xBaconPrice;
                        break;
                    default:
                        break;
                }
            }

            if (item.Extra != null)
            {
                foreach (var extra in item.Extra)
                {
                    switch (extra.ToLower())
                    {
                        case "fries":
                            item.Price += friesPrice;
                            break;
                        case "soft drink":
                            item.Price += softDrinkPrice;
                            break;
                        default:
                            break;
                    }
                }
            }

            AppyDiscont(item);

            return item;
        }

        // Method to apply discounts to the order based on selected items
        private void AppyDiscont(OrderItemFullModel item)
        {
            bool selectSandwich = item.Sandwich != null;
            bool selectFries = item.Extra?.Contains("fries") ?? false;
            bool selectSoftDrink = item.Extra?.Contains("soft drink") ?? false;

            //Logic to appy the Disconts
            if (selectSandwich && selectFries && selectSoftDrink)
            {
                item.Price *= 0.8; 
            }
            else if (selectSandwich && selectSoftDrink)
            {
                item.Price *= 0.85; 
            }
            else if (selectSandwich && selectFries)
            {
                item.Price *= 0.9; 
            }
        }

        // Method to get the current order list
        public OrderListModel GetOrderListModel()
        {
            return _orderListModel;
        }


        // Method to update orders in the order list
        public void UpdateOrder(List<PutOrderListModel> updateOrderModels)
        {
            if (updateOrderModels == null)
            {
                return;
            }

            foreach (var updateOrderModel in updateOrderModels)
            {
                var orderItemToUpdate = _orderListModel.OrderItems.FirstOrDefault(item => item.Id == updateOrderModel.Id);

                if (orderItemToUpdate != null)
                {
                    orderItemToUpdate.Sandwich = updateOrderModel.Sandwich;
                    orderItemToUpdate.Extra = updateOrderModel.Extra;
                    AmountCharged(orderItemToUpdate);
                }
            }
        }

        // Method to delete an order from the order list
        public void DeleteOrder(int orderId)
        {
            var orderToRemove = _orderListModel.OrderItems.FirstOrDefault(item => item.Id == orderId);

            if (orderToRemove != null)
            {
                _orderListModel.OrderItems.Remove(orderToRemove);
            }
        }

        // Method to check if there are multiple selections from the same category
        private bool HasMultipleSelectionsFromSameCategory(string sandwich, List<string> extras)
        {
            bool selectSandwich = !string.IsNullOrEmpty(sandwich);
            bool selectFries = extras?.Contains("fries") ?? false;
            bool selectSoftDrink = extras?.Contains("soft drink") ?? false;

            int countSandwich = selectSandwich ? 1 : 0;
            int countFries = selectFries ? 1 : 0;
            int countSoftDrink = selectSoftDrink ? 1 : 0;

            return (countSandwich + countFries + countSoftDrink > 3) || (countFries > 1) || (countSoftDrink > 1);
        }
    }
}