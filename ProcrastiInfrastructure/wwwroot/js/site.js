function getToastContainer() {
    let container = document.getElementById('global-toast-container');
    if (!container) {
        container = document.createElement('div');
        container.id = 'global-toast-container';

        container.className = 'toast toast-end toast-bottom z-[9999] p-4 flex flex-col gap-3';
        document.body.appendChild(container);
    }
    return container;
}

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
    const container = getToastContainer();
    const alertBox = document.createElement('div');

    alertBox.className = "alert bg-background border-2 border-accent text-text shadow-xl flex items-center gap-4 w-92 p-4 rounded-xl transition-all duration-500 opacity-0 translate-y-5";

    const iconSrc = data.icon ? data.icon : '/images/achievements/default-icon.png';
    alertBox.innerHTML = `
        <div class="w-14 h-14 bg-secondary/20 rounded-full shrink-0 overflow-hidden flex items-center justify-center">
            <img src="${iconSrc}" class="w-full h-full object-cover object-center" />
        </div>
        <div class="flex flex-col text-left">
            <span class="text-accent font-bold text-sm uppercase tracking-wider">Досягнення розблоковано!</span>
            <h5 class="font-bold text-base mt-0.5">${data.title}</h5>
            <p class="text-xs text-primary mt-1 whitespace-pre-wrap">${data.description}</p>
        </div>
    `;

    container.appendChild(alertBox);

    requestAnimationFrame(() => {
        alertBox.classList.remove('opacity-0', 'translate-y-5');
        alertBox.classList.add('opacity-100', 'translate-y-0');
    });

    setTimeout(() => {
        alertBox.classList.remove('opacity-100', 'translate-y-0');
        alertBox.classList.add('opacity-0', 'translate-y-5');
        setTimeout(() => {
            alertBox.remove();
            if (container.children.length === 0) {
                container.remove();
            }
        }, 500);
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
    const container = getToastContainer();
    const alertBox = document.createElement('div');

    alertBox.className = "alert bg-gradient-to-br from-text to-primary border-2 border-amber-400 text-background shadow-[0_0_20px_rgba(251,191,36,0.3)] flex items-center gap-4 w-92 p-4 rounded-xl transition-all duration-500 opacity-0 translate-y-5";

    alertBox.innerHTML = `
        <div class="w-12 h-12 bg-white/20 rounded-full shrink-0 flex justify-center items-center text-2xl">
            👑
        </div>
        <div class="flex flex-col text-left">
            <span class="text-amber-400 font-bold text-xs uppercase tracking-widest">Титул розблоковано!</span>
            <h5 class="font-bold text-lg mt-0.5 drop-shadow-md">${data.name}</h5>
        </div>
    `;

    container.appendChild(alertBox);

    requestAnimationFrame(() => {
        alertBox.classList.remove('opacity-0', 'translate-y-5');
        alertBox.classList.add('opacity-100', 'translate-y-0');
    });

    setTimeout(() => {
        alertBox.classList.remove('opacity-100', 'translate-y-0');
        alertBox.classList.add('opacity-0', 'translate-y-5');
        setTimeout(() => {
            alertBox.remove();
            if (container.children.length === 0) {
                container.remove();
            }
        }, 500);
    }, 5000);
}

function triggerRickroll(element) {
    if (typeof tryUnlockAchievement === 'function') {
        tryUnlockAchievement("NGGYU");
    }

    element.classList.remove('border-secondary/40', 'bg-secondary/10', 'grayscale', 'opacity-60');
    element.classList.add('border-accent', 'bg-background', 'shadow-sm');

    const statusText = element.querySelector('.mt-4 p');
    if (statusText) {
        statusText.innerText = "Unlocked";

        statusText.classList.remove('text-primary', 'italic');
        statusText.classList.add('text-accent', 'font-bold', 'uppercase', 'tracking-wider');
    }

    element.style.transition = "all 0.5s ease-in-out";
    element.style.transform = "rotate(360deg) scale(0.1)";
    element.style.opacity = "0";

    setTimeout(() => {
        window.open('/Achievements/Rickroll', '_blank');

        element.style.transform = "";
        element.style.opacity = "";
        element.style.transition = "";
    }, 500);
}