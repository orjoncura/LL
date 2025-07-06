// public/service-worker.js

self.addEventListener('push', (event) => {
  event.waitUntil(
    (async () => {
      let data = {
        title: 'Default Title',
        body: 'Default Body',
        icon: '/icon.png',
        badge: '/badge.png',
      };

      try {
        if (event.data) {
          const json = event.data.json();
          data.title = json.title || data.title;
          data.body = json.body || data.body;
          data.icon = json.icon || data.icon;
          data.badge = json.badge || data.badge;
        }
      } catch (e) {
        data.body = await event.data.text(); // fallback for plain text
      }

      return self.registration.showNotification(data.title, {
        body: data.body,
        icon: data.icon,
        badge: data.badge,
        data: {
          url: data.url || '/',
        },
      });
    })()
  );
});

self.addEventListener('notificationclick', (event) => {
  event.notification.close();
  const targetUrl = event.notification.data || '/';

  event.waitUntil(
    clients.matchAll({ type: 'window', includeUncontrolled: true }).then((clientList) => {
      for (const client of clientList) {
        if (client.url === targetUrl && 'focus' in client) {
          return client.focus();
        }
      }
      if (clients.openWindow) {
        return clients.openWindow(targetUrl);
      }
    })
  );
});
