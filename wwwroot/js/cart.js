let cart = [];
let totalAmount = 0;
let currentOpenOrder = null;
let selectedPaymentMethod = "Card";
let isCartExpanded = true;
let lastPaidOrderId = null;

let currentTipType = 'none';
let currentTipValue = 0;
let statusPollingInterval = null;
let currentPromoDiscount = 0;

function setupCartForPayment(order) {
    currentOpenOrder = order;
    cart = [];
    totalAmount = order.totalAmount;
    currentTipType = 'none';
    currentTipValue = 0;
}

function setupCartForNewOrder() {
    currentOpenOrder = null;
    cart = [];
    totalAmount = 0;
}

function addToCart(id, name, price) {
    if (currentOpenOrder && currentOpenOrder.orderStatus === "WaitingForPayment") {
        tg.showAlert("Ви вже попросили рахунок, дозамовлення неможливе.");
        return;
    }
    const existingItem = cart.find(item => item.menuItemId === id);
    if (existingItem) existingItem.quantity += 1; else cart.push({ menuItemId: id, name: name, priceAtPurchase: price, quantity: 1 });
    updateCartTotals();
    updateCartUI();
    tg.HapticFeedback.impactOccurred('light');
}

function changeQuantity(id, delta) {
    const item = cart.find(item => item.menuItemId === id);
    if (item) { item.quantity += delta; if (item.quantity <= 0) cart = cart.filter(i => i.menuItemId !== id); }
    updateCartTotals();
    renderCartPage();
}

function updateCartTotals() {
    try {
        let baseSum = 0;
        let promoDiscount = 0;
        let expandedItems = [];

        cart.forEach(item => {
            baseSum += (item.priceAtPurchase * item.quantity);

            // Надійний пошук категорії для акції
            if (typeof allMenuItems !== 'undefined' && allMenuItems.length > 0) {
                const fullItem = allMenuItems.find(m => m.id == item.menuItemId);
                if (fullItem) {
                    for (let i = 0; i < item.quantity; i++) {
                        expandedItems.push({ price: item.priceAtPurchase, categoryId: fullItem.categoryId });
                    }
                }
            }
        });

        // Застосовуємо акції (Безпечна перевірка)
        if (typeof activePromotions !== 'undefined' && Array.isArray(activePromotions) && activePromotions.length > 0) {
            const byCategory = expandedItems.reduce((acc, obj) => {
                acc[obj.categoryId] = acc[obj.categoryId] || [];
                acc[obj.categoryId].push(obj.price);
                return acc;
            }, {});

            for (const catId in byCategory) {
                let prices = byCategory[catId].sort((a, b) => b - a); // Сортуємо по спаданню

                let promo = activePromotions.find(p => p.categoryId == catId || p.categoryId == null);

                if (promo && promo.isActive) {
                    if (promo.promoType === "1+1=3") {
                        for (let i = 2; i < prices.length; i += 3) {
                            promoDiscount += prices[i]; // Третій - безкоштовний
                        }
                    } else if (promo.promoType === "Discount") {
                        prices.forEach(p => {
                            promoDiscount += p * (promo.discountPercent / 100);
                        });
                    }
                }
            }
        }

        currentPromoDiscount = promoDiscount;

        if (currentOpenOrder) {
            totalAmount = currentOpenOrder.totalAmount + baseSum - promoDiscount;
        } else {
            totalAmount = baseSum - promoDiscount;
        }

        if (totalAmount < 0) totalAmount = 0;

    } catch (e) {
        console.error("Помилка розрахунку акції:", e);
        // Запасний варіант якщо щось пішло не так
        let fallbackSum = cart.reduce((sum, item) => sum + (item.priceAtPurchase * item.quantity), 0);
        totalAmount = currentOpenOrder ? currentOpenOrder.totalAmount + fallbackSum : fallbackSum;
        currentPromoDiscount = 0;
    }
}

function updateCartUI() {
    const cartBar = document.getElementById('cart-bar');
    const isCartPageActive = document.getElementById('page-cart').classList.contains('active');

    if (cart.length > 0 && !isCartPageActive) {
        document.getElementById('total-qty').innerText = cart.reduce((sum, item) => sum + item.quantity, 0);

        // ВАЖЛИВО: Виводимо загальну суму з урахуванням знижок!
        document.getElementById('total-price').innerText = Math.round(totalAmount) + " ₴";

        cartBar.classList.add('visible');
    } else { cartBar.classList.remove('visible'); }
}

function toggleCartItems() {
    const wrapper = document.getElementById('cart-items-wrapper');
    const arrow = document.getElementById('cart-toggle-arrow');
    isCartExpanded = !isCartExpanded;
    if (isCartExpanded) { wrapper.classList.add('expanded'); arrow.innerText = '▲'; }
    else { wrapper.classList.remove('expanded'); arrow.innerText = '▼'; }
}

function setPaymentMethod(method) {
    selectedPaymentMethod = method;
    document.querySelectorAll('.payment-btn').forEach(b => b.classList.remove('active'));

    if (method === 'Mono') {
        document.getElementById('btn-pay-mono').classList.add('active');
        document.getElementById('cart-main-btn').innerText = 'Оплатити через Mono';
    } else {
        document.getElementById(`btn-pay-${method.toLowerCase()}`).classList.add('active');
        document.getElementById('cart-main-btn').innerText = 'Покликати офіціанта';
    }
}

function setTip(value, type) {
    currentTipType = type;
    currentTipValue = value;
    document.querySelectorAll('.tip-btn').forEach(b => b.classList.remove('active'));

    if (type === 'none') {
        document.getElementById('btn-tip-none').classList.add('active');
        document.getElementById('custom-tip-input').style.display = 'none';
        document.getElementById('custom-tip-input').value = '';
    } else if (type === 'percent') {
        document.getElementById('btn-tip-' + value).classList.add('active');
        document.getElementById('custom-tip-input').style.display = 'none';
    }
    updateCartFinalPrice();
}

function setTipCustom() {
    currentTipType = 'custom';
    document.querySelectorAll('.tip-btn').forEach(b => b.classList.remove('active'));
    document.getElementById('btn-tip-custom').classList.add('active');
    const input = document.getElementById('custom-tip-input');
    input.style.display = 'block';
    input.focus();
    updateCartFinalPrice();
}

function renderCartPage() {
    const list = document.getElementById('cart-page-list');
    list.innerHTML = '';

    const monoContainer = document.getElementById("mono-pay-container");
    if (monoContainer) { monoContainer.innerHTML = ''; monoContainer.style.display = 'none'; }
    const mainBtn = document.getElementById('cart-main-btn');
    if (mainBtn) mainBtn.style.display = 'block';

    if (cart.length === 0 && !currentOpenOrder) {
        list.innerHTML = '<p style="text-align: center; color: var(--text-muted); margin-top: 10px;">Кошик порожній</p>';
        document.getElementById('cart-page-total').innerText = '0 ₴';
        document.getElementById('cart-page-final').innerText = '0 ₴';
        document.getElementById('payment-section').style.display = 'none';
        document.getElementById('table-input-block').style.display = 'flex';

        if (mainBtn) mainBtn.innerText = 'Відправити на кухню';
        stopStatusPolling();
        return;
    }

    if (currentOpenOrder && currentOpenOrder.orderStatus === "WaitingForPayment") {
        document.getElementById('cart-items-wrapper').style.display = 'none';
        document.querySelector('.cart-collapse-header').style.display = 'none';
        document.getElementById('cart-summary-box').style.display = 'none';
        document.getElementById('table-input-block').style.display = 'none';
        if (mainBtn) mainBtn.style.display = 'none';

        const wTitle = document.getElementById('waiter-title');
        const wDesc = document.getElementById('waiter-desc');

        wTitle.innerText = 'Запит відправлено! 🛎';
        wDesc.innerHTML = 'Офіціант вже йде до вас.<br>Будь ласка, очікуйте біля столику.';

        document.getElementById('waiter-screen').style.display = 'block';
        document.getElementById('cart-header-title').innerText = "Очікування";

        startStatusPolling();
        return;
    }

    stopStatusPolling();

    document.getElementById('cart-items-wrapper').style.display = 'block';
    document.querySelector('.cart-collapse-header').style.display = 'flex';
    document.getElementById('cart-summary-box').style.display = 'block';
    document.getElementById('waiter-screen').style.display = 'none';

    if (currentOpenOrder) {
        currentOpenOrder.orderItems.forEach(item => {
            const card = document.createElement('div');
            card.className = 'cart-item-card';
            card.style.opacity = '0.6';
            card.innerHTML = `<div class="cart-item-info"><span class="cart-item-name">${item.menuItem ? item.menuItem.name : "Товар"}</span><span class="cart-item-price">${item.priceAtPurchase} ₴</span></div><div class="qty-controls"><span style="font-weight: 800;">${item.quantity} шт.</span></div>`;
            list.appendChild(card);
        });
    }

    cart.forEach(item => {
        const card = document.createElement('div');
        card.className = 'cart-item-card';
        card.innerHTML = `
            <div class="cart-item-info"><span class="cart-item-name">${item.name}</span><span class="cart-item-price">${item.priceAtPurchase} ₴</span></div>
            <div class="qty-controls">
                <button class="qty-btn" onclick="changeQuantity(${item.menuItemId}, -1)">-</button>
                <span style="font-weight: 800;">${item.quantity}</span>
                <button class="qty-btn" onclick="changeQuantity(${item.menuItemId}, 1)">+</button>
            </div>
        `;
        list.appendChild(card);
    });

    // Вивід суми кошика з урахуванням акцій
    if (currentPromoDiscount > 0 && !currentOpenOrder) {
        document.getElementById('cart-page-total').innerHTML = `${Math.round(totalAmount)} ₴ <span class="promo-badge" style="background: #10b981; color: white; padding: 2px 6px; border-radius: 6px; font-size: 11px; margin-left: 5px;">Акція: -${Math.round(currentPromoDiscount)} ₴</span>`;
    } else {
        document.getElementById('cart-page-total').innerText = `${Math.round(totalAmount)} ₴`;
    }

    if (currentOpenOrder && cart.length > 0) {
        document.getElementById('cart-header-title').innerText = "Дозамовлення";
        document.getElementById('payment-section').style.display = 'none';
        document.getElementById('table-input-block').style.display = 'none';

        if (mainBtn) mainBtn.innerText = 'Відправити ДОЗАМОВЛЕННЯ на кухню';

        document.getElementById('cart-final-label').innerText = 'Разом:';
        document.getElementById('cart-page-final').innerText = `${Math.round(totalAmount)} ₴`;
        if (!isCartExpanded) toggleCartItems();
    }
    else if (currentOpenOrder && cart.length === 0) {
        document.getElementById('cart-header-title').innerText = "Оплата рахунку";
        document.getElementById('payment-section').style.display = 'block';
        document.getElementById('table-input-block').style.display = 'none';

        document.getElementById('cart-final-label').innerText = 'Разом:';
        setPaymentMethod('Card');
        mountMonoButton();
        setTip(0, 'none');
        updateCartFinalPrice();
        if (isCartExpanded) toggleCartItems();
    }
    else {
        document.getElementById('cart-header-title').innerText = "Кошик";
        document.getElementById('payment-section').style.display = 'none';
        document.getElementById('table-input-block').style.display = 'flex';

        if (mainBtn) mainBtn.innerText = 'Відправити на кухню';

        document.getElementById('cart-final-label').innerText = 'Разом:';
        document.getElementById('cart-page-final').innerText = `${Math.round(totalAmount)} ₴`;
        if (!isCartExpanded) toggleCartItems();
    }
}

function updateCartFinalPrice() {
    if (!currentOpenOrder) return;
    let bonusInput = document.getElementById('cart-page-bonus-input');
    let bonusToUse = 0;
    if (bonusInput) {
        bonusToUse = parseFloat(bonusInput.value) || 0;
        if (typeof customerBonuses !== 'undefined' && bonusToUse > customerBonuses) bonusToUse = customerBonuses;
        if (bonusToUse > totalAmount) bonusToUse = totalAmount;
        bonusInput.value = bonusToUse;
    }

    let tipAmount = 0;
    if (currentTipType === 'percent') tipAmount = totalAmount * (currentTipValue / 100);
    else if (currentTipType === 'custom') tipAmount = parseFloat(document.getElementById('custom-tip-input').value) || 0;
    if (tipAmount < 0) tipAmount = 0;

    let finalAmount = totalAmount - bonusToUse + tipAmount;
    document.getElementById('cart-page-final').innerText = `${Math.round(finalAmount)} ₴`;
}

async function processOrderAction() {
    const btn = document.getElementById('cart-main-btn');
    if (btn && btn.disabled) return;

    const isAddingItems = (currentOpenOrder && cart.length > 0);
    const isPaying = (currentOpenOrder && cart.length === 0);
    const originalText = btn ? btn.innerText : 'Обробка...';

    if (btn) {
        btn.disabled = true;
        btn.innerText = 'Обробка...';
    }

    try {
        if (isPaying) {
            if (selectedPaymentMethod === 'Mono') {
                await initFakeMonoPay();
            } else {
                await callWaiter();
            }
        }
        else if (isAddingItems) {
            await sendAddItemsToKitchen();
        }
        else {
            await sendToKitchen();
        }
    } finally {
        if (btn) { btn.disabled = false; btn.innerText = originalText; }
    }
}

async function sendToKitchen() {
    let tInput = document.getElementById('cart-table-number');
    let tNumber = tInput && tInput.value.trim() !== "" ? tInput.value.trim() : null;

    if (tNumber) {
        let isOccupied = await apiCheckTable(tNumber, dbCustomerId);
        if (isOccupied) return tg.showAlert("Цей столик вже має активне замовлення! Зверніться до баристи.");
    }

    const orderData = {
        customerId: dbCustomerId, orderType: tNumber ? "В закладі" : "Самовиніс", tableNumber: tNumber,
        totalAmount: totalAmount, bonusUsed: 0, paymentStatus: "Unpaid", paymentMethod: "None",
        orderItems: cart.map(item => ({ menuItemId: item.menuItemId, quantity: item.quantity, priceAtPurchase: item.priceAtPurchase }))
    };

    try {
        await apiCreateOrder(orderData);
        tg.showAlert('✅ Замовлення прийнято кухнею!');
        cart = []; updateCartUI();
        await loadAllData();
        switchTab('home', document.querySelectorAll('.nav-item')[0]);
    } catch (error) { tg.showAlert('❌ Помилка при створенні замовлення.'); }
}

async function sendAddItemsToKitchen() {
    const itemsData = cart.map(item => ({ menuItemId: item.menuItemId, quantity: item.quantity, priceAtPurchase: item.priceAtPurchase }));
    try {
        await apiAddItemsToOrder(currentOpenOrder.id, itemsData);
        tg.showAlert('✅ Дозамовлення прийнято!');
        cart = []; updateCartUI();
        await loadAllData();
        switchTab('home', document.querySelectorAll('.nav-item')[0]);
    } catch (error) { tg.showAlert('❌ Помилка дозамовлення.'); }
}

async function callWaiter() {
    let bonusToUse = parseFloat(document.getElementById('cart-page-bonus-input').value) || 0;
    let finalTip = 0;
    if (currentTipType === 'percent') finalTip = totalAmount * (currentTipValue / 100);
    else if (currentTipType === 'custom') finalTip = parseFloat(document.getElementById('custom-tip-input').value) || 0;

    try {
        await apiCallWaiter(currentOpenOrder.id, { bonusUsed: bonusToUse, paymentMethod: selectedPaymentMethod, tipAmount: finalTip });
        lastPaidOrderId = currentOpenOrder.id;
        await loadAllData();
        renderCartPage();
    } catch (error) {
        tg.showAlert(`❌ Помилка: ${error.message}`);
    }
}

async function initFakeMonoPay() {
    let bonusToUse = parseFloat(document.getElementById('cart-page-bonus-input').value) || 0;
    let finalTip = 0;
    if (currentTipType === 'percent') finalTip = totalAmount * (currentTipValue / 100);
    else if (currentTipType === 'custom') finalTip = parseFloat(document.getElementById('custom-tip-input').value) || 0;

    const finalPrice = totalAmount - bonusToUse + finalTip;

    document.getElementById('cart-items-wrapper').style.display = 'none';
    document.querySelector('.cart-collapse-header').style.display = 'none';
    document.getElementById('cart-summary-box').style.display = 'none';
    document.getElementById('cart-main-btn').style.display = 'none';

    const container = document.getElementById("mono-pay-container");
    container.style.display = "block";
    container.innerHTML = `
        <div class="fake-pay-widget">
            <div class="fake-pay-header">
                <svg viewBox="0 0 24 24" class="fake-pay-logo"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-1 14H9V8h2v8zm4 0h-2V8h2v8z"/></svg>
                <span>Оплата замовлення</span>
            </div>
            <div class="fake-pay-amount">${finalPrice.toFixed(2)} ₴</div>
            <div class="fake-pay-card">
                <div class="fake-pay-chip"></div>
                <div class="fake-pay-dots">•••• •••• •••• 1234</div>
            </div>
            <div class="fake-pay-loader" id="fake-pay-loader">
                <div class="spinner"></div>
                <p style="color: var(--text-main); font-weight: 700;">Обробка платежу...</p>
            </div>
            <button class="fake-pay-btn" id="fake-pay-confirm-btn" onclick="confirmFakePay(${bonusToUse}, ${finalTip})">Підтвердити оплату</button>
            <button class="fake-pay-cancel" id="fake-pay-cancel-btn" onclick="cancelFakePay()">Скасувати</button>
        </div>
    `;
}

function cancelFakePay() {
    document.getElementById("mono-pay-container").style.display = "none";
    document.getElementById('cart-main-btn').style.display = "block";
    renderCartPage();
}

async function confirmFakePay(bonusUsed, tipAmount) {
    document.getElementById('fake-pay-confirm-btn').style.display = 'none';
    document.getElementById('fake-pay-cancel-btn').style.display = 'none';
    const loader = document.getElementById('fake-pay-loader');
    loader.style.display = 'flex';

    try {
        await apiFakeOnlinePay(currentOpenOrder.id, { bonusUsed: bonusUsed, paymentMethod: "Online", tipAmount: tipAmount });
        lastPaidOrderId = currentOpenOrder.id;

        setTimeout(async () => {
            loader.innerHTML = `
                <svg class="fake-pay-success-icon" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/></svg>
                <p style="color: var(--text-main); font-weight: 800; font-size: 18px; margin-top: 10px;">Оплачено успішно!</p>
            `;
            setTimeout(async () => {
                document.getElementById("mono-pay-container").style.display = "none";
                await loadAllData();
                switchTab('rating');
            }, 2000);
        }, 1500);

    } catch (error) {
        loader.innerHTML = `
            <svg class="fake-pay-error-icon" viewBox="0 0 24 24"><path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm5 13.59L15.59 17 12 13.41 8.41 17 7 15.59 10.59 12 7 8.41 8.41 7 12 10.59 15.59 7 17 8.41 13.41 12 17 15.59z"/></svg>
            <p style="font-weight: 800; font-size: 16px; margin-top: 10px; color: var(--danger-color);">${error.message}</p>
        `;
        setTimeout(() => {
            cancelFakePay();
        }, 3000);
    }
}

function startStatusPolling() {
    if (statusPollingInterval) return;
    statusPollingInterval = setInterval(async () => {
        try {
            const openOrder = await apiGetOpenOrder(dbCustomerId);
            if (openOrder === null) {
                stopStatusPolling();
                await loadAllData();
                switchTab('rating');
            }
        } catch (e) {
        }
    }, 3000);
}

function stopStatusPolling() {
    if (statusPollingInterval) {
        clearInterval(statusPollingInterval);
        statusPollingInterval = null;
    }
}

async function rateOrder(stars) {
    const starsElements = document.querySelectorAll('.rating-star');
    starsElements.forEach((star, index) => {
        if (index < stars) star.classList.add('filled');
        else star.classList.remove('filled');
    });

    if (lastPaidOrderId) {
        try {
            await apiRateOrder(lastPaidOrderId, stars);
            document.getElementById('rating-thanks').style.opacity = '1';
            setTimeout(async () => {
                document.getElementById('rating-thanks').style.opacity = '0';
                await loadAllData();
                switchTab('history', document.querySelectorAll('.nav-item')[2]);
            }, 1500);
        } catch (e) { console.error(e); }
    }
}

function mountMonoButton() {
    const container = document.getElementById("real-mono-btn-container");
    if (!container || container.hasChildNodes()) return;

    if (window.MonoPay) {
        try {
            const { button } = window.MonoPay.init({
                keyId: "dummy_key_id",
                signature: "dummy_signature",
                requestId: "dummy_request_id",
                payloadBase64: "eyJhbW91bnQiOjUwMDAsImNjeSI6OTgwLCJtZXJjaGFudFBheW1JbmZvIjp7InJlZmVyZW5jZSI6ImR1bW15In19",
                ui: {
                    buttonType: "base",
                    theme: "dark",
                    corners: "rounded"
                }
            });

            button.style.pointerEvents = 'none';
            button.style.width = '100%';

            container.appendChild(button);
        } catch (e) {
            console.error("MonoPay init error:", e);
            container.innerHTML = '<div style="text-align:center; font-weight: 800; padding: 12px; color: white;">plata by mono</div>';
        }
    } else {
        container.innerHTML = '<div style="text-align:center; font-weight: 800; padding: 12px; color: white;">plata by mono</div>';
    }
}