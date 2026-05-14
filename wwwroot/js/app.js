const tg = window.Telegram.WebApp;
tg.expand();
tg.setHeaderColor('#f8f6f4');

let allMenuItems = [];
let activePromotions = []; // ТЕПЕР ОГОЛОШЕНО КОРЕКТНО!
let customerOrdersCount = 0;
let menuCategoriesList = [];
let currentCatIndex = 0;
let scannedTableNumber = null;
let myReferralCode = "";
let dbCustomerId = null;
let customerBonuses = 0;
let customerProfileData = null;

async function initApp() {
    const startParam = tg.initDataUnsafe?.start_param;
    if (startParam && startParam.startsWith('table_')) { scannedTableNumber = startParam.split('_')[1]; }
    const loadDataPromise = loadAllData();
    const splashDelay = new Promise(resolve => setTimeout(resolve, 2000));
    await Promise.all([splashDelay, loadDataPromise]);

    document.getElementById('page-home').classList.add('active');
    document.getElementById('splash-screen').classList.add('hidden');

    document.querySelectorAll('.modal-overlay').forEach(overlay => {
        overlay.addEventListener('click', (e) => {
            if (e.target === overlay) closeModal(overlay.id);
        });
    });

    initSwipeLogic();
}

async function loadAllData() {
    const user = tg.initDataUnsafe?.user;
    let telegramId = 123456789;
    let fullName = "Гість (Браузер)";
    if (user) { telegramId = user.id; fullName = `${user.first_name} ${user.last_name || ''}`.trim(); document.getElementById('user-greeting').innerText = `Привіт, ${user.first_name}! ☕️`; }

    try {
        const customer = await apiSyncCustomer({ telegramId, fullName, phoneNumber: "Не вказано", totalSpent: 0, bonusBalance: 0, loyaltyLevel: "Standard" });
        dbCustomerId = customer.id;
        customerBonuses = customer.bonusBalance;
        myReferralCode = customer.referralCode;

        customerProfileData = customer;

        document.getElementById('main-balance').innerText = `${customerBonuses} ₴`;
        document.getElementById('my-ref-code').innerText = myReferralCode;

        if (customerBonuses > 0) {
            const bonusBox = document.getElementById('cart-page-bonus-box');
            if (bonusBox) bonusBox.style.display = 'flex';
            const availBonuses = document.getElementById('cart-page-available-bonuses');
            if (availBonuses) availBonuses.innerText = customerBonuses.toFixed(2);
            const bonusInput = document.getElementById('cart-page-bonus-input');
            if (bonusInput) bonusInput.max = customerBonuses;
        }

        document.getElementById('profile-name').innerText = customer.fullName;
        document.getElementById('profile-phone').innerText = customer.phoneNumber === "Не вказано" ? "+38(067) 000-00-00" : customer.phoneNumber;

        const dobBadge = document.getElementById('profile-dob');
        if (customer.dateOfBirth) {
            const d = new Date(customer.dateOfBirth);
            dobBadge.innerHTML = `🎁 ${d.toLocaleDateString('uk-UA')}`;
            dobBadge.style.background = 'var(--bg-color)';
            dobBadge.style.color = 'var(--text-main)';
        } else {
            dobBadge.innerHTML = `🎁 Додати день народження`;
            dobBadge.style.background = '#f5ebe6';
            dobBadge.style.color = 'var(--primary-color)';
        }

    } catch (error) { console.error("Помилка синхронізації користувача:", error); }

    try {
        allMenuItems = await apiGetMenuItems(dbCustomerId);

        // Безпечне завантаження акцій
        try {
            activePromotions = await apiGetPromotions();
        } catch (e) {
            activePromotions = [];
        }

        renderCategories();
        await renderNews();
    } catch (e) { console.error("Помилка завантаження меню:", e); }

    try {
        const orders = await apiGetCustomerOrders(dbCustomerId);
        customerOrdersCount = orders.filter(o => o.paymentStatus === 'Paid').length;
        renderHistory(orders);
        updateLoyaltyWidget();
    } catch (e) {
        console.error("Помилка завантаження історії:", e);
        document.getElementById('history-list').innerHTML = '<p style="text-align: center; color: var(--text-muted); margin-top: 40px;">Помилка завантаження історії.</p>';
    }

    try {
        await checkOpenOrder();
    } catch (e) { console.error("Помилка перевірки відкритого замовлення:", e); }
}

async function renderNews() {
    try {
        const news = await apiGetNews();
        const homeCont = document.getElementById('home-news-container');
        const pageCont = document.getElementById('page-news-container');

        if (!homeCont || !pageCont) return;

        homeCont.innerHTML = '';
        pageCont.innerHTML = '';

        if (news.length === 0) {
            homeCont.innerHTML = '<p style="text-align: center; color: var(--text-muted); width: 100%; margin-top: 10px;">Новин поки немає</p>';
            pageCont.innerHTML = '<p style="text-align: center; color: var(--text-muted); margin-top: 20px;">Новин поки немає</p>';
            return;
        }

        news.forEach(n => {
            const bg = n.imageUrl ? n.imageUrl : 'https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=600&h=300&fit=crop';

            homeCont.innerHTML += `
                <div class="news-card" onclick="switchTab('news', document.querySelectorAll('.nav-item')[0])">
                    <div class="news-img-box" style="background-image: url('${bg}');"></div>
                    <div class="news-text-box">${n.title}</div>
                </div>`;

            pageCont.innerHTML += `
                <div class="vertical-news-card">
                    <div class="news-img-box" style="background-image: url('${bg}');"></div>
                    <div class="news-text-box">${n.title}</div>
                </div>`;
        });
    } catch (e) { console.error("Помилка завантаження новин:", e); }
}

async function checkOpenOrder() {
    const openOrder = await apiGetOpenOrder(dbCustomerId);
    const banner = document.getElementById('active-order-banner');

    if (openOrder) {
        document.getElementById('active-order-amount').innerText = openOrder.totalAmount;
        banner.style.display = 'flex';
        if (typeof setupCartForPayment === 'function') setupCartForPayment(openOrder);
    } else {
        banner.style.display = 'none';
        if (typeof setupCartForNewOrder === 'function') setupCartForNewOrder();
    }
}

function updateLoyaltyWidget() {
    let rankName = "Новачок"; let progress = 0;
    if (customerOrdersCount < 15) { rankName = "Новачок"; progress = (customerOrdersCount / 15) * 100; document.getElementById('tier-1').classList.add('active'); }
    else if (customerOrdersCount < 50) { rankName = "Кавоман"; progress = ((customerOrdersCount - 15) / 35) * 100; document.getElementById('tier-2').classList.add('active'); }
    else { rankName = "Еліта"; progress = 100; document.getElementById('tier-3').classList.add('active'); }

    if (progress === 0 && customerOrdersCount > 0) progress = 5;
    const progressCircle = document.getElementById('loyalty-progress');
    progressCircle.style.setProperty('--progress', `${progress}%`);
    progressCircle.querySelector('span').innerText = `${Math.floor(progress)}%`;
    document.getElementById('loyalty-rank').innerText = rankName;
}

function renderHistory(orders) {
    const historyList = document.getElementById('history-list');
    historyList.innerHTML = '';
    const completedOrders = orders.filter(o => o.paymentStatus === 'Paid');

    if (completedOrders.length === 0) { historyList.innerHTML = '<p style="text-align: center; color: var(--text-muted); margin-top: 40px;">У вас ще немає оплачених замовлень.</p>'; }
    else {
        completedOrders.forEach(order => {
            const orderDate = new Date(order.createdAt || new Date());
            const formattedDate = `${orderDate.toLocaleDateString('uk-UA')} ${orderDate.toLocaleTimeString('uk-UA', { hour: '2-digit', minute: '2-digit' })}`;

            let itemsText = '';
            if (order.orderItems && order.orderItems.length > 0) {
                itemsText = order.orderItems.map(oi => `${oi.menuItem ? oi.menuItem.name : 'Товар'} x${oi.quantity}`).join('<br>');
            }

            let rating = order.rating || 5;
            let starsHtml = `<div class="stars-container">`;
            for (let i = 1; i <= 5; i++) {
                starsHtml += `<svg class="${i <= rating ? 'filled' : ''}" viewBox="0 0 24 24"><path d="M12 17.27L18.18 21l-1.64-7.03L22 9.24l-7.19-.61L12 2 9.19 8.63 2 9.24l5.46 4.73L5.82 21z"/></svg>`;
            }
            starsHtml += `</div>`;

            const card = document.createElement('div');
            card.className = 'history-card';
            card.innerHTML = `
                <div class="history-header-row"><div class="history-number">№${order.id}</div>${starsHtml}</div>
                <div class="history-date">${formattedDate}</div>
                <div class="history-items">${itemsText}</div>
                <div class="history-finances">
                    <div class="finance-row"><span class="finance-label">Сплачено:</span><span class="finance-value val-minus">-${order.finalAmount} ₴</span></div>
                    <div class="finance-row"><span class="finance-label">Списано:</span><span class="finance-value val-minus">-${order.bonusUsed} бонусів</span></div>
                </div>
            `;
            historyList.appendChild(card);
        });
    }
}

function renderCategories() {
    const categoryScroll = document.getElementById('category-scroll');
    categoryScroll.innerHTML = '';
    menuCategoriesList = [];
    allMenuItems.forEach(item => { if (item.category && !menuCategoriesList.includes(item.category.name)) menuCategoriesList.push(item.category.name); });
    if (menuCategoriesList.length === 0) return;

    let defaultCat = menuCategoriesList[0];
    menuCategoriesList.forEach((cat, index) => {
        const btn = document.createElement('div');
        btn.className = `category-tab ${index === currentCatIndex ? "active" : ""}`;
        btn.innerText = cat;
        btn.onclick = () => changeCategory(index);
        categoryScroll.appendChild(btn);
    });
    renderMenu(menuCategoriesList[currentCatIndex], '');
}

function changeCategory(newIndex) {
    if (newIndex === currentCatIndex || newIndex < 0 || newIndex >= menuCategoriesList.length) return;
    let animClass = newIndex > currentCatIndex ? 'slide-left' : 'slide-right';
    currentCatIndex = newIndex;
    let tabs = document.querySelectorAll('.category-tab');
    tabs.forEach(t => t.classList.remove('active'));
    if (tabs[currentCatIndex]) { tabs[currentCatIndex].classList.add('active'); tabs[currentCatIndex].scrollIntoView({ behavior: "smooth", inline: "center", block: "nearest" }); }
    renderMenu(menuCategoriesList[currentCatIndex], animClass);
}

function renderMenu(categoryName, animClass) {
    const menuList = document.getElementById('menu-list');
    menuList.className = 'menu-container'; void menuList.offsetWidth; if (animClass) menuList.classList.add(animClass);
    menuList.innerHTML = '';
    let filteredItems = allMenuItems.filter(item => item.category && item.category.name === categoryName);

    filteredItems.forEach(item => {
        const imageUrl = item.imageUrl ? item.imageUrl : "https://images.unsplash.com/photo-1559525839-b184a4d698c7?w=300&h=300&fit=crop";
        let desc = item.description || "";

        let shortDesc = desc;
        if (desc.length > 25) {
            shortDesc = desc.substring(0, 25) + '... <span class="text-details" onclick="openProductDetails(' + item.id + ')">деталі</span>';
        } else if (desc.length > 0) {
            shortDesc = desc + ' <span class="text-details" onclick="openProductDetails(' + item.id + ')">деталі</span>';
        }

        let tagsHtml = '<div class="menu-tags">';
        if (item.containsGluten) tagsHtml += '<span class="menu-tag tag-g">Г</span>';
        if (item.containsLactose) tagsHtml += '<span class="menu-tag tag-l">Л</span>';
        if (item.isSpicy) tagsHtml += '<span class="menu-tag tag-s">🌶</span>';
        if (item.isNew) tagsHtml += '<span class="menu-tag menu-tag-n"><span class="tag-n-star">⭐</span> Новинка</span>';
        tagsHtml += '</div>';

        let weightHtml = item.weightOrVolume ? `<div class="item-weight">${item.weightOrVolume}</div>` : '';

        const likedClass = item.isLikedByUser ? 'liked' : '';
        const likeBtnHtml = `
            <button class="item-like-btn ${likedClass}" id="like-btn-${item.id}" onclick="toggleLike(${item.id})">
                <svg viewBox="0 0 24 24"><path d="M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 2 5.42 4.42 3 7.5 3c1.74 0 3.41.81 4.5 2.09C13.09 3.81 14.76 3 16.5 3 19.58 3 22 5.42 22 8.5c0 3.78-3.4 6.86-8.55 11.54L12 21.35z"/></svg>
                <div class="item-like-count" id="like-count-${item.id}">${item.totalLikes}</div>
            </button>
        `;

        const card = document.createElement('div');
        card.className = 'menu-card';
        card.innerHTML = `
            <div class="menu-card-img" style="background-image: url('${imageUrl}');" onclick="openProductDetails(${item.id})"></div>
            ${likeBtnHtml}
            <div class="menu-card-content">
                <div>
                    <div class="item-name" onclick="openProductDetails(${item.id})">${item.name}</div>
                    ${weightHtml}
                    ${tagsHtml}
                    <div class="item-desc">${shortDesc}</div>
                </div>
                <div class="item-bottom">
                    <div class="item-price">${item.price} ₴</div>
                    <button class="add-btn" onclick="addToCart(${item.id}, '${item.name.replace(/'/g, "\\'")}', ${item.price})">Додати</button>
                </div>
            </div>
        `;
        menuList.appendChild(card);
    });
}

async function toggleLike(itemId) {
    const item = allMenuItems.find(i => i.id === itemId);
    if (!item) return;
    if (event) event.stopPropagation();

    const btn = document.getElementById(`like-btn-${itemId}`);
    const countLabel = document.getElementById(`like-count-${itemId}`);

    btn.style.transform = 'scale(1.2)';
    setTimeout(() => btn.style.transform = 'scale(1)', 150);

    try {
        if (!item.isLikedByUser) {
            btn.classList.add('liked');
            const res = await apiLikeMenuItem(itemId, dbCustomerId);
            item.isLikedByUser = true;
            item.totalLikes = res.totalLikes;
            countLabel.innerText = res.totalLikes;
        } else {
            btn.classList.remove('liked');
            const res = await apiUnlikeMenuItem(itemId, dbCustomerId);
            item.isLikedByUser = false;
            item.totalLikes = res.totalLikes;
            countLabel.innerText = res.totalLikes;
        }
    } catch (e) {
        if (item.isLikedByUser) btn.classList.add('liked'); else btn.classList.remove('liked');
        countLabel.innerText = item.totalLikes;
        tg.showAlert("Помилка обробки лайка. Спробуйте пізніше.");
    }
}

function openProductDetails(id) {
    const item = allMenuItems.find(i => i.id === id);
    if (!item) return;

    const imageUrl = item.imageUrl ? item.imageUrl : "https://images.unsplash.com/photo-1559525839-b184a4d698c7?w=500&h=500&fit=crop";
    document.getElementById('pd-img').style.backgroundImage = `url('${imageUrl}')`;
    document.getElementById('pd-title').innerText = item.name;

    document.getElementById('pd-like-count').innerText = item.totalLikes;
    const likeIcon = document.getElementById('pd-like-icon');
    if (item.isLikedByUser) {
        likeIcon.style.fill = 'var(--primary-color)';
    } else {
        likeIcon.style.fill = 'none';
    }

    document.getElementById('pd-weight').innerText = item.weightOrVolume ? item.weightOrVolume : "";
    document.getElementById('pd-desc').innerText = item.description || "Опис відсутній.";
    document.getElementById('pd-price').innerText = `${item.price} ₴`;

    let tagsHtml = '';
    if (item.containsGluten) tagsHtml += '<span class="modal-tag tag-g"><span class="modal-tag-icon">🌾</span> Містить глютен</span>';
    if (item.containsLactose) tagsHtml += '<span class="modal-tag tag-l"><span class="modal-tag-icon">🥛</span> Містить лактозу</span>';
    if (item.isSpicy) tagsHtml += '<span class="modal-tag tag-s"><span class="modal-tag-icon">🌶</span> Гостре</span>';

    if (item.isNew) tagsHtml += '<span class="modal-tag tag-n" style="color: var(--danger-color); background: rgba(230,57,70,0.1);"><span class="modal-tag-icon">⭐</span> Новинка</span>';

    document.getElementById('pd-tags').innerHTML = tagsHtml;

    const addBtn = document.getElementById('pd-add-btn');
    addBtn.onclick = () => {
        addToCart(item.id, item.name, item.price);
        closeModal('product-details-modal');
    };

    openModal('product-details-modal');
}

function initSwipeLogic() {
    const menuPage = document.getElementById('page-menu');
    let touchStartX = 0;
    menuPage.addEventListener('touchstart', e => { touchStartX = e.changedTouches[0].screenX; }, { passive: true });
    menuPage.addEventListener('touchend', e => {
        let touchEndX = e.changedTouches[0].screenX;
        if (touchEndX < touchStartX - 60) changeCategory(currentCatIndex + 1);
        if (touchEndX > touchStartX + 60) changeCategory(currentCatIndex - 1);
    }, { passive: true });
}

function switchTab(tabName, element) {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    document.querySelectorAll('.page').forEach(page => page.classList.remove('active'));
    document.getElementById('page-' + tabName).classList.add('active');

    const bottomNav = document.getElementById('bottom-nav');
    const cartBar = document.getElementById('cart-bar');

    if (tabName === 'cart' || tabName === 'rating') {
        bottomNav.classList.add('hidden');
        cartBar.style.display = 'none'; cartBar.classList.remove('visible');
        if (tabName === 'cart' && typeof renderCartPage === "function") renderCartPage();
    } else {
        bottomNav.classList.remove('hidden');
        cartBar.style.display = 'flex';
        if (typeof updateCartUI === "function") updateCartUI();
    }
    if (element) { document.querySelectorAll('.nav-item').forEach(btn => btn.classList.remove('active')); element.classList.add('active'); }
}

function openModal(modalId) { document.getElementById(modalId).classList.add('active'); }
function closeModal(modalId) { document.getElementById(modalId).classList.remove('active'); }

function requestPhoneFromTelegram() {
    if (tg.requestContact) {
        tg.requestContact((shared) => {
            if (shared) {
                tg.showAlert("Дякуємо! Ваш номер надіслано боту. Оновлюємо профіль...");
                closeModal('phone-modal');
                setTimeout(loadAllData, 2000);
            } else {
                document.getElementById('tg-phone-btn').style.display = 'none';
                document.getElementById('manual-phone-block').style.display = 'block';
            }
        });
    } else {
        document.getElementById('tg-phone-btn').style.display = 'none';
        document.getElementById('manual-phone-block').style.display = 'block';
    }
}

async function savePhoneNumberManual() {
    const phoneInput = document.getElementById('phone-input').value;
    if (phoneInput.length < 10) return tg.showAlert("Будь ласка, введіть коректний номер.");

    try {
        tg.MainButton.showProgress();
        await apiUpdatePhone(dbCustomerId, phoneInput);
        document.getElementById('profile-phone').innerText = phoneInput;
        closeModal('phone-modal');
        tg.showAlert("Номер збережено!");
    } catch (e) { tg.showAlert("Помилка збереження."); }
    finally { tg.MainButton.hideProgress(); }
}

function copyRefCode() {
    navigator.clipboard.writeText(myReferralCode).then(() => {
        tg.showAlert(`Код ${myReferralCode} скопійовано! Надішліть його друзям.`);
    });
}

async function applyReferral() {
    const refInput = document.getElementById('ref-input').value.trim();
    if (refInput.length < 6) return tg.showAlert("Код має містити мінімум 6 символів.");

    try {
        tg.MainButton.showProgress();
        await apiApplyReferral(dbCustomerId, refInput);
        closeModal('ref-modal');
        tg.showAlert("Код прийнято! 🎁 50 бонусів будуть нараховані після оплати вашого першого замовлення.");
        await loadAllData();
    } catch (e) {
        tg.showAlert(e.message);
    } finally {
        tg.MainButton.hideProgress();
    }
}

function openEditProfileModal() {
    if (!customerProfileData) return;

    document.getElementById('edit-name-input').value = customerProfileData.fullName || '';

    const dobInput = document.getElementById('edit-dob-input');
    if (customerProfileData.dateOfBirth) {
        const d = new Date(customerProfileData.dateOfBirth);
        dobInput.value = d.toISOString().split('T')[0];
        dobInput.disabled = true;
        dobInput.style.opacity = '0.6';
    } else {
        dobInput.value = '';
        dobInput.disabled = false;
        dobInput.style.opacity = '1';
    }

    openModal('edit-profile-modal');
}

async function saveProfileData() {
    const newName = document.getElementById('edit-name-input').value.trim();
    let newDob = document.getElementById('edit-dob-input').value;

    if (newName.length < 2) return tg.showAlert("Ім'я надто коротке.");

    let dobToSave = null;
    if (newDob && !customerProfileData.dateOfBirth) {
        dobToSave = new Date(newDob + 'T00:00:00Z').toISOString();
    }

    try {
        tg.MainButton.showProgress();
        const req = { FullName: newName };
        if (dobToSave) req.DateOfBirth = dobToSave;

        await apiUpdateProfile(dbCustomerId, req);

        closeModal('edit-profile-modal');
        tg.showAlert("Профіль оновлено!");
        await loadAllData();
    } catch (e) {
        tg.showAlert("Помилка збереження.");
    } finally {
        tg.MainButton.hideProgress();
    }
}

document.addEventListener('touchstart', function (e) {
    if (document.activeElement && document.activeElement.tagName === 'INPUT' && e.target.tagName !== 'INPUT') {
        document.activeElement.blur();
    }
});

initApp();