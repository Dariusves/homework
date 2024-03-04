using System.Net.Http.Json;
using IntusHomework.DTOs;

namespace IntusHomework.Client.Pages
{
    public partial class OrdersPage
    {
        private HttpClient Http = new HttpClient { BaseAddress = new Uri("https://localhost:7281") };

        //ORDERS
        private OrderDTO newOrder = new OrderDTO();
        private OrderDTO selectedOrder;
        private OrderDTO editableOrder;
        private List<OrderDTO> orders;
        private bool showEditOrderForm = false;
        private bool showCreateOrderForm = false;

        private void SelectOrder(OrderDTO order)
        {
            selectedOrder = order;
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            orders = await Http.GetFromJsonAsync<List<OrderDTO>>("api/orders");
        }


        private void CreateOrder()
        {
            showCreateOrderForm = true;
        }

        private async Task SaveOrderAsync()
        {
            if (newOrder != null)
            {
                var response = await Http.PostAsJsonAsync("api/orders", newOrder);

                if (response.IsSuccessStatusCode)
                {
                    newOrder = new OrderDTO();
                    showCreateOrderForm = false;
                    await LoadOrdersAsync();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error saving order: " + errorMessage);
                }
            }
        }

        private void ModifyOrder(OrderDTO order)
        {
            editableOrder = new OrderDTO
            {
                OrderId = order.OrderId,
                Name = order.Name,
                State = order.State,
                Windows = new List<WindowDTO>(order.Windows)
            };

            showEditOrderForm = true;
        }

        private async Task DeleteOrder(int orderId)
        {
            await Http.DeleteAsync($"api/orders/{orderId}");
            await LoadOrdersAsync();
            selectedOrder = null;
            selectedWindow = null;
        }

        private async Task HandleEditOrder()
        {
            var response = await Http.PutAsJsonAsync($"api/orders/{editableOrder.OrderId}", editableOrder);
            if (response.IsSuccessStatusCode)
            {
                showEditOrderForm = false;
                await LoadOrdersAsync();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error updating order: " + errorMessage);
            }
        }

        //WINDOWS
        private WindowDTO windowToEdit { get; set; }
        private WindowDTO newWindow = new WindowDTO();
        private WindowDTO selectedWindow;
        private bool showAddWindowForm = false;

        private async Task LoadWindowsForOrder(int orderId)
        {
            OrderDTO updatedOrder = await Http.GetFromJsonAsync<OrderDTO>($"api/orders/{orderId}");
            selectedOrder = updatedOrder;
        }

        private void SelectWindow(WindowDTO window)
        {
            selectedWindow = window;
        }

        private void ShowAddWindowForm()
        {
            newWindow = new WindowDTO();
            showAddWindowForm = true;
        }

        private async Task ModifyWindow(WindowDTO window)
        {
            windowToEdit = window;
            StateHasChanged();
        }

        private async Task DeleteWindow(int windowId)
        {
            _ = Http.DeleteAsync($"api/windows/{windowId}");

            await Task.Delay(500);

            await LoadWindowsForOrder(selectedOrder.OrderId);
            await LoadOrdersAsync();

            selectedWindow = null;
        }

        private async Task HandleAddWindow()
        {
            newWindow.OrderId = selectedOrder.OrderId;

            var response = await Http.PostAsJsonAsync("api/windows", newWindow);
            if (response.IsSuccessStatusCode)
            {
                showAddWindowForm = false;
                newWindow = new WindowDTO();
                await LoadWindowsForOrder(selectedOrder.OrderId);
                await LoadOrdersAsync();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error saving window: " + errorMessage);
            }
        }

        private async Task HandleModifyWindow()
        {
            var response = await Http.PutAsJsonAsync($"api/windows/{windowToEdit.WindowId}", windowToEdit);
            if (response.IsSuccessStatusCode)
            {
                windowToEdit = null;
                await LoadOrdersAsync();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error modifying window: " + errorMessage);
            }
        }

        private void CancelEdit()
        {
            windowToEdit = null;
        }

        //SUB ELEMENTS

        private SubElementDTO newSubElement = new SubElementDTO();
        private SubElementDTO subElementToEdit;
        private bool showAddSubElementForm = false;

        private void HandleEditSubElement(SubElementDTO subElementDTO)
        {
            subElementToEdit = subElementDTO;
        }

        private async Task EditSubElement()
        {
            var response = await Http.PutAsJsonAsync($"api/subelements/{subElementToEdit.SubElementId}", subElementToEdit);
            if (response.IsSuccessStatusCode)
            {
                await LoadSubElementsForWindow(selectedWindow.WindowId);
                subElementToEdit = null;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error updating subelement: " + errorMessage);
            }
        }

        private async Task DeleteSubElement(int subElementId)
        {
            var response = await Http.DeleteAsync($"api/subelements/{subElementId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadSubElementsForWindow(selectedWindow.WindowId);
                await CalculateTotalSubElementsForWindow(selectedWindow);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error deleting subelement: " + errorMessage);
            }
        }

        private void ShowAddSubElementForm()
        {
            showAddSubElementForm = true;
        }

        private async Task HandleAddSubElement()
        {
            if (newSubElement != null && selectedWindow != null)
            {
                newSubElement.WindowId = selectedWindow.WindowId;

                var response = await Http.PostAsJsonAsync("api/subelements", newSubElement);
                if (response.IsSuccessStatusCode)
                {
                    showAddSubElementForm = false;
                    newSubElement = new SubElementDTO();
                    await LoadSubElementsForWindow(selectedWindow.WindowId);
                    await CalculateTotalSubElementsForWindow(selectedWindow);
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error saving subelement: " + errorMessage);
                }
            }
        }

        private async Task LoadSubElementsForWindow(int windowId)
        {
            var windowResponse = await Http.GetFromJsonAsync<WindowDTO>($"api/windows/{windowId}");
            if (windowResponse != null)
            {
                selectedWindow.SubElements = windowResponse.SubElements;
            }
        }

        private async Task CalculateTotalSubElementsForWindow(WindowDTO selectedWindow)
        {
            if (selectedWindow == null || selectedWindow.SubElements == null)
            {
                return;
            }

            int totalSubElements = selectedWindow.SubElements.Count();

            selectedWindow.TotalSubElements = totalSubElements;

            var response = await Http.PutAsJsonAsync($"api/windows/{selectedWindow.WindowId}", selectedWindow);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Error updating window: " + errorMessage);
            }
        }
    }
}
