async function tryUnlockAchievement(achievementCode) {
    try {
        const response = await fetch('/Achievements/Unlock', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(achievementCode)
        });

        if (response.ok) {
            const achivementData = await response.json();
            showAchivementPopup(achivementData);
        }
    } catch (error) {
        console.error('Error unlocking achievement:', error);
    }
}

function showAchivementPopup(data) {
    const toast = document.createElement('div');
    toast.style.cssText = "position: fixed; bottom: 30px; right: 30px; width: 320px; background-color: #222; color: white; border-radius: 10px; padding: 15px; display: flex; align-items: center; gap: 15px; box-shadow: 0 5px 15px rgba(0,0,0,0.5); z-index: 9999; transition: opacity 0.5s; opacity: 0;";

    const iconSrc = data.icon ? data.icon : '/images/achievements/default-icon.png';
    toast.innerHTML = `
        <div style="width: 60px; height: 60px; flex-shrink: 0; background-color: #444; border-radius: 50%; overflow: hidden;">
            <img src="${iconSrc}" style="width: 100%; height: 100%; object-fit: cover; object-position: center;" />
        </div>
        <div>
            <small style="color: #4caf50; font-weight: bold; text-transform: uppercase;">Achievement Unlocked!</small>
            <h5 style="margin: 5px 0 0 0;">${data.title}</h5>
            <p style="margin: 5px 0 0 0; font-size: 12px; color: #ccc; white-space: pre-wrap;">${data.description}</p>
        </div>
    `;

    document.body.appendChild(toast);
    setTimeout(() => { toast.style.opacity = '1'; }, 10);

    setTimeout(() => {
        toast.style.opacity = '0';
        setTimeout(() => { toast.remove(); }, 500);
    }, 5000);
}

async function tryUnlockTitle(titleCode) {
    try {
        const response = await fetch('/Titles/Unlock', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(titleCode)
        });

        if (response.ok) {
            const titleData = await response.json();
            showTitlePopup(titleData);
        }
    } catch (error) {
        console.error('Error unlocking title:', error);
    }
}

function showTitlePopup(data) {
    const toast = document.createElement('div');
    toast.style.cssText = "position: fixed; bottom: 30px; right: 30px; width: 320px; background: linear-gradient(135deg, #2b1055, #7597de); color: white; border: 1px solid #ffd700; border-radius: 10px; padding: 15px; display: flex; align-items: center; gap: 15px; box-shadow: 0 5px 15px rgba(255, 215, 0, 0.3); z-index: 9999; transition: opacity 0.5s; opacity: 0;";

    toast.innerHTML = `<div style="width: 50px; height: 50px; flex-shrink: 0; background-color: rgba(255,255,255,0.2); border-radius: 50%; display: flex; justify-content: center; align-items: center; font-size: 24px;">
            👑
        </div>
        <div>
            <small style="color: #ffd700; font-weight: bold; text-transform: uppercase; letter-spacing: 1px;">New Title Unlocked!</small>
            <h5 style="margin: 5px 0 0 0; text-shadow: 1px 1px 2px rgba(0,0,0,0.5);">${data.name}</h5>
        </div>
    `;

    document.body.appendChild(toast);

    setTimeout(() => { toast.style.opacity = '1'; }, 10);

    setTimeout(() => {
        toast.style.opacity = '0';
        setTimeout(() => { toast.remove(); }, 500);
    }, 5000);
}