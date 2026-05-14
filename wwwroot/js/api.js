const API_BASE_URL = '/api';

async function fetchApi(endpoint, options = {}) {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, options);

        if (!response.ok) {
            const errorText = await response.text();
            let errorJson;
            try { errorJson = JSON.parse(errorText); } catch (e) { }
            throw new Error(errorJson?.message || errorText || `API error: ${response.status}`);
        }

        const contentType = response.headers.get("content-type");
        if (contentType && contentType.includes("application/json")) {
            return await response.json();
        }
        return null;
    } catch (error) {
        console.error(`API Fetch Error (${endpoint}):`, error);
        throw error;
    }
}

async function apiSyncCustomer(customerData) { return await fetchApi('/Customers/sync', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(customerData) }); }

// ВИПРАВЛЕНО: Відправляємо об'єкт для телефону
async function apiUpdatePhone(customerId, phoneNumber) { return await fetchApi(`/Customers/${customerId}/UpdatePhone`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ phoneNumber: phoneNumber }) }); }

// ВИПРАВЛЕНО: Відправляємо об'єкт для реферального коду
async function apiApplyReferral(customerId, referralCode) { return await fetchApi(`/Customers/${customerId}/ApplyReferral`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({ code: referralCode }) }); }

async function apiUpdateProfile(customerId, profileData) { return await fetchApi(`/Customers/${customerId}/UpdateProfile`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(profileData) }); }
async function apiGetMenuItems(customerId) { return await fetchApi(`/MenuItems?customerId=${customerId}`); }
async function apiGetNews() { return await fetchApi('/AdminDashboard/News'); }
async function apiCreateOrder(orderData) { return await fetchApi('/Orders', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(orderData) }); }

async function apiGetOpenOrder(customerId) {
    try {
        const response = await fetch(`${API_BASE_URL}/Orders/Customer/${customerId}/Open`);
        if (response.status === 404) return null;
        if (!response.ok) throw new Error('Network error');
        return await response.json();
    } catch (e) {
        return null;
    }
}

async function apiGetCustomerOrders(customerId) { return await fetchApi(`/Orders/customer/${customerId}`); }
async function apiCheckTable(tableNumber, customerId) { return await fetchApi(`/Orders/Table/${tableNumber}/Check?currentCustomerId=${customerId}`); }
async function apiAddItemsToOrder(orderId, itemsData) { return await fetchApi(`/Orders/${orderId}/AddItems`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(itemsData) }); }
async function apiCallWaiter(orderId, paymentData) { return await fetchApi(`/Orders/${orderId}/CallWaiter`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(paymentData) }); }
async function apiFakeOnlinePay(orderId, paymentData) { return await fetchApi(`/Orders/${orderId}/FakeOnlinePay`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(paymentData) }); }
async function apiRateOrder(orderId, rating) { return await fetchApi(`/Orders/${orderId}/Rate`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(rating) }); }
async function apiLikeMenuItem(itemId, customerId) { return await fetchApi(`/MenuItems/${itemId}/like?customerId=${customerId}`, { method: 'POST' }); }
async function apiUnlikeMenuItem(itemId, customerId) { return await fetchApi(`/MenuItems/${itemId}/unlike?customerId=${customerId}`, { method: 'DELETE' }); }

// Акції
async function apiGetPromotions() { return await fetchApi('/AdminDashboard/Promotions'); }