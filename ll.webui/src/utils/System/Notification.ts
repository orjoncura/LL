export async function SendLocalNotifications(title: string, body: string): Promise<string> {

    const notificationPermission = Notification.permission;

    if (!('Notification' in window)) 
      return 'Notifications are not supported.';
    
    if (notificationPermission === 'denied') 
      return 'Notification permission denied. Please enable it in browser settings.';
    

    if (notificationPermission === 'default') {
      const permission = await Notification.requestPermission();

      if (permission !== 'granted') 
        return 'Permission not granted for local notifications.';
      
    }

    // Create and display the local notification
    new Notification(title, {
      body: body,
      icon: '/icons/icon-192x192.png', // Path to your PWA icon
    });

    return 'Local notification displayed!';
}