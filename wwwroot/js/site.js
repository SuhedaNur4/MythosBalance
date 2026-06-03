document.addEventListener('DOMContentLoaded', function () {
    const notifModal = document.getElementById('notificationModal');
    if (notifModal) {
        const hasNotifications = notifModal.dataset.hasNotifications === 'true';
        if (hasNotifications) {
            setTimeout(function () {
                const modal = new bootstrap.Modal(notifModal);
                modal.show();
            }, 800);
        }
    }

    document.querySelectorAll('.btn-dismiss-notification').forEach(function (btn) {
        btn.addEventListener('click', async function () {
            const notifId = this.dataset.notifId;
            const item = document.getElementById('notif-item-' + notifId);

            try {
                const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                const response = await fetch('/Home/DismissNotification', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': token || ''
                    },
                    body: 'notificationId=' + notifId + '&__RequestVerificationToken=' + (token || '')
                });

                if (response.ok && item) {
                    item.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                    item.style.opacity = '0';
                    item.style.transform = 'translateX(10px)';
                    setTimeout(() => item.remove(), 300);
                }
            } catch (err) {
                console.error('Bildirim kapatılamadı:', err);
            }
        });
    });

    const btnDismissAll = document.getElementById('btn-dismiss-all-notifs');
    if (btnDismissAll) {
        btnDismissAll.addEventListener('click', async function () {
        });
    }

    if (notifModal) {
        notifModal.addEventListener('hidden.bs.modal', async function () {
            try {
                const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
                await fetch('/Home/DismissAllNotifications', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': token || ''
                    },
                    body: '__RequestVerificationToken=' + (token || '')
                });
            } catch (err) {
                console.error('Tüm bildirimler kapatılamadı:', err);
            }
        });
    }

    const successAlert = document.getElementById('successAlert');
    if (successAlert) {
        setTimeout(function () {
            successAlert.style.transition = 'opacity 0.5s ease';
            successAlert.style.opacity = '0';
            setTimeout(() => successAlert.remove(), 500);
        }, 3500);
    }

    const currentPath = window.location.pathname.toLowerCase();
    document.querySelectorAll('.nav-link-mythos').forEach(function (link) {
        const href = link.getAttribute('href')?.toLowerCase() || '';
        if (href !== '/' && currentPath.startsWith(href)) {
            link.classList.add('active');
        } else if (href === '/' && currentPath === '/') {
            link.classList.add('active');
        }
    });

    document.querySelectorAll('.admin-nav-link').forEach(function (link) {
        const href = link.getAttribute('href')?.toLowerCase() || '';
        if (currentPath.startsWith(href)) {
            link.classList.add('active');
        }
    });
});

