const sendBtn = document.getElementById('send-btn');
const chatInput = document.querySelector('.chat-input');
const chatMessages = document.querySelector('.chat-messages');
const chatToggle = document.getElementById('chat-toggle');
const chatContainer = document.getElementById('chat-container');
const messageInput = document.getElementById('message-input');
const settingsBtn = document.querySelector('.chat-header .settings');
const settingsMenu = document.getElementById('settings-menu');
const soundToggle = document.getElementById('sound-toggle');
const themeToggle = document.getElementById('theme-toggle');
const whatsappBtn = document.getElementById('whatsapp-btn');
const attachBtn = document.getElementById('attach-btn');
const fileInput = document.getElementById('file-input');
const whatsappModal = document.getElementById('whatsapp-modal');

let lastTimeDisplayed = null;
let isSoundEnabled = true;
let selectedLanguage = null;
let selectedTopic = null;
let selectedSubTopic = null;
let inactivityTimer = null;

// Load saved messages, theme, sound settings, language, and topic
document.addEventListener('DOMContentLoaded', () => {
    console.log('DOM fully loaded and parsed');

    const savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.forEach(msg => {
        const messageContainer = document.createElement('div');
        messageContainer.classList.add('message-container');
        const messageElement = document.createElement('div');
        messageElement.classList.add('message', msg.type === 'received' ? 'received' : '');
        if (msg.isImage) {
            const img = document.createElement('img');
            img.src = msg.text;
            messageElement.appendChild(img);
        } else {
            messageElement.textContent = msg.text;
        }
        messageContainer.appendChild(messageElement);
        chatMessages.appendChild(messageContainer);
    });
    chatMessages.scrollTop = chatMessages.scrollHeight;

    if (localStorage.getItem('theme') === 'dark') {
        document.body.classList.add('dark-theme');
        chatContainer.classList.add('dark-theme');
        themeToggle.checked = true;
    }

    isSoundEnabled = localStorage.getItem('soundEnabled') !== 'false';
    soundToggle.checked = isSoundEnabled;
    updateSoundLabel();

    selectedLanguage = localStorage.getItem('language');
    selectedTopic = localStorage.getItem('selectedTopic');
    selectedSubTopic = localStorage.getItem('selectedSubTopic');

    chatInput.style.display = 'flex';
    if (!selectedLanguage) {
        console.log('No language selected, waiting for chat to open');
    } else {
        showGreeting();
        showTopicSelection();
    }

    messageInput.addEventListener('input', () => {
        sendBtn.style.display = messageInput.value.trim() ? 'block' : 'none';
    });

    if (selectedLanguage && selectedTopic) {
        startInactivityTimer();
    }
});

// Toggle sound
function updateSoundLabel() {
    const soundLabel = soundToggle.parentElement.previousElementSibling;
    soundLabel.textContent = `🔊 Звук ${isSoundEnabled ? 'вкл.' : 'выкл.'}`;
}

soundToggle.addEventListener('change', () => {
    isSoundEnabled = soundToggle.checked;
    localStorage.setItem('soundEnabled', isSoundEnabled);
    updateSoundLabel();
});

// Toggle theme
themeToggle.addEventListener('change', () => {
    document.body.classList.toggle('dark-theme');
    chatContainer.classList.toggle('dark-theme');
    localStorage.setItem('theme', document.body.classList.contains('dark-theme') ? 'dark' : 'light');
});

// WhatsApp button
whatsappBtn.addEventListener('click', () => {
    settingsMenu.style.display = 'none';
    showWhatsAppModal();
});

// Show WhatsApp modal
function showWhatsAppModal() {
    const modalTitle = document.getElementById('whatsapp-modal-title');
    const modalText = document.getElementById('whatsapp-modal-text');
    const modalInstruction = document.getElementById('whatsapp-modal-instruction');
    const modalFooter = document.getElementById('whatsapp-modal-footer');

    const translations = {
        'kz': {
            title: 'WhatsApp-та жалғастыру',
            text: 'WhatsApp аккаунтыңызда әңгімені жалғастырыңыз. Сіз кез келген уақытта қайта орала аласыз.',
            instruction: 'QR-кодты сканерлеңіз және WhatsApp-та пайда болатын хабарламаны жіберіңіз.',
            footer: 'Осы құрылғыда WhatsApp-ты ашыңыз.'
        },
        'ru': {
            title: 'Продолжить в WhatsApp',
            text: 'Перейдите к разговору в своём аккаунте WhatsApp. Вы можете вернуться в любое время.',
            instruction: 'Отсканируйте QR-код и отправьте сообщение, которое появится в WhatsApp.',
            footer: 'Откройте WhatsApp на этом устройстве.'
        },
        'en': {
            title: 'Continue on WhatsApp',
            text: 'Continue the conversation in your WhatsApp account. You can return at any time.',
            instruction: 'Scan the QR code and send the message that appears in WhatsApp.',
            footer: 'Open WhatsApp on this device.'
        }
    };

    const lang = selectedLanguage || 'ru';
    modalTitle.textContent = translations[lang].title;
    modalText.textContent = translations[lang].text;
    modalInstruction.textContent = translations[lang].instruction;
    modalFooter.textContent = translations[lang].footer;

    whatsappModal.style.display = 'flex';
}

// Close WhatsApp modal
function closeWhatsAppModal() {
    whatsappModal.style.display = 'none';
}

// Show/hide settings menu
settingsBtn.addEventListener('click', () => {
    settingsMenu.style.display = settingsMenu.style.display === 'none' || !settingsMenu.style.display ? 'flex' : 'none';
});

// Show greeting
function showGreeting() {
    const greetings = {
        'kz': 'Сәлем! Мен VibePlace Bot, қалай көмектесе аламын?',
        'ru': 'Привет! Я VibePlace Bot, чем могу помочь?',
        'en': 'Hello! I am VibePlace Bot, how can I assist you?'
    };
    const greetingText = greetings[selectedLanguage] || greetings['ru'];

    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message', 'received');
    messageElement.textContent = greetingText;
    messageContainer.appendChild(messageElement);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: greetingText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
}

// Show language selection
function showLanguageSelection() {
    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message', 'received');
    messageElement.textContent = 'Please select your language / Пожалуйста, выберите язык / Тілді таңдаңыз';

    const languageDiv = document.createElement('div');
    languageDiv.classList.add('language-selection');

    const kzBtn = document.createElement('button');
    kzBtn.classList.add('language-btn');
    kzBtn.textContent = 'Қазақша';
    kzBtn.onclick = () => selectLanguage('kz');

    const ruBtn = document.createElement('button');
    ruBtn.classList.add('language-btn');
    ruBtn.textContent = 'Русский';
    ruBtn.onclick = () => selectLanguage('ru');

    const enBtn = document.createElement('button');
    enBtn.classList.add('language-btn');
    enBtn.textContent = 'English';
    enBtn.onclick = () => selectLanguage('en');

    languageDiv.appendChild(kzBtn);
    languageDiv.appendChild(ruBtn);
    languageDiv.appendChild(enBtn);

    messageContainer.appendChild(messageElement);
    messageContainer.appendChild(languageDiv);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: 'Please select your language / Пожалуйста, выберите язык / Тілді таңдаңыз', type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
}

// Show topic selection
function showTopicSelection() {
    const topicPrompts = {
        'kz': 'Сұрауыңыздың тақырыбын таңдаңыз:',
        'ru': 'Выберите тему вашего запроса:',
        'en': 'Please select the topic of your request:'
    };
    const promptText = topicPrompts[selectedLanguage] || topicPrompts['ru'];

    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message', 'received');
    messageElement.textContent = promptText;

    const topicDiv = document.createElement('div');
    topicDiv.classList.add('topic-selection');

    const topicTemplate = document.getElementById(`topic-template-${selectedLanguage}`);
    if (topicTemplate) {
        const topicContent = topicTemplate.content.cloneNode(true);
        topicDiv.appendChild(topicContent);
    }

    messageContainer.appendChild(messageElement);
    messageContainer.appendChild(topicDiv);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: promptText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
}

// Handle language selection
function selectLanguage(lang) {
    selectedLanguage = lang;
    localStorage.setItem('language', lang);

    const langText = lang === 'kz' ? 'Қазақша' : lang === 'ru' ? 'Русский' : 'English';
    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message');
    messageElement.textContent = langText;
    messageContainer.appendChild(messageElement);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: langText, type: 'sent' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    showGreeting();
    showTopicSelection();
}

// Handle topic selection
function selectTopic(topic) {
    selectedTopic = topic;
    localStorage.setItem('selectedTopic', topic);

    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message');
    messageElement.textContent = topic;
    messageContainer.appendChild(messageElement);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: topic, type: 'sent' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    const topicCategories = {
        'kz': {
            'Іс-шара түрі': 'event-type',
            'Орын түрі': 'place-type',
            'Аудитория бойынша': 'audience',
            'Танымал': 'popular'
        },
        'ru': {
            'Тип мероприятия': 'event-type',
            'Тип места': 'place-type',
            'По аудитории': 'audience',
            'Популярное': 'popular'
        },
        'en': {
            'Event Type': 'event-type',
            'Place Type': 'place-type',
            'By Audience': 'audience',
            'Popular': 'popular'
        }
    };

    const category = topicCategories[selectedLanguage]?.[topic];
    if (category) {
        showSubTopicSelection(category);
    } else if (topic === (selectedLanguage === 'kz' ? 'Ассистент бот' : selectedLanguage === 'ru' ? 'Ассистент бот' : 'Assistant Bot')) {
        const assistantPrompts = {
            'kz': 'Сіз Ассистент ботты таңдадыңыз, қалай көмектесе аламын?',
            'ru': 'Вы выбрали Ассистент бот, чем могу помочь?',
            'en': 'You have selected Assistant Bot, how can I assist you?'
        };
        const promptText = assistantPrompts[selectedLanguage] || assistantPrompts['ru'];

        const assistantPromptContainer = document.createElement('div');
        assistantPromptContainer.classList.add('message-container');
        const assistantPromptElement = document.createElement('div');
        assistantPromptElement.classList.add('message', 'received');
        assistantPromptElement.textContent = promptText;
        assistantPromptContainer.appendChild(assistantPromptElement);
        chatMessages.appendChild(assistantPromptContainer);
        chatMessages.scrollTop = chatMessages.scrollHeight;

        savedMessages.push({ text: promptText, type: 'received' });
        localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
    }

    startInactivityTimer();
}

// Show subtopic selection
function showSubTopicSelection(category) {
    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');

    const subTopicTemplate = document.getElementById(`sub-topic-template-${selectedLanguage}-${category}`);
    if (subTopicTemplate) {
        const subTopicContent = subTopicTemplate.content.cloneNode(true);
        messageContainer.appendChild(subTopicContent);
    }

    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    const promptText = subTopicTemplate.querySelector('.message.received').textContent;
    savedMessages.push({ text: promptText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
}

// Handle subtopic selection (called from HTML)
function selectSubTopic(subTopic) {
    selectedSubTopic = subTopic;
    localStorage.setItem('selectedSubTopic', subTopic);

    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message');
    messageElement.textContent = subTopic;
    messageContainer.appendChild(messageElement);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: subTopic, type: 'sent' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    const subTopicResponses = {
        'kz': {
            'Туған күн': 'Туған күнді ұйымдастыру үшін бізде бірнеше тамаша идеялар бар! Мысалы, тақырыптық кештер, аниматорлар немесе ерекше орындар. Сізге қандай формат ұнайды?',
            'Үйлену тойы': 'Үйлену тойы – бұл ерекше оқиға! Біз сізге банкет залдарын, декораторларды немесе фотографтарды табуға көмектесе аламыз. Сізге қандай қызмет қажет?',
            'Корпоратив': 'Корпоративтік іс-шаралар командаңызды біріктіреді! Біз сізге тимбилдинг идеяларын, кейтерингті немесе ойын-сауық бағдарламаларын ұсына аламыз. Сізге қандай формат керек?',
            'Іскерлік кездесулер': 'Іскерлік кездесулер үшін біз конференц-залдарды, техникалық жабдықтарды және кофе-брейктерді ұйымдастыруды ұсынамыз. Сізге қандай қызмет қажет?',
            'Мерейтой': 'Мерейтой – бұл маңызды мереке! Біз сізге банкет залдарын, музыкалық бағдарламаларды немесе тосынсыйларды ұйымдастыруға көмектесе аламыз. Сізге қандай формат ұнайды?',
            'Мейрамхана': 'Мейрамханада іс-шара өткізу – тамаша таңдау! Біз сізге үстелдерді брондауға, мәзір таңдауға немесе арнайы декорға көмектесе аламыз. Сізге не қажет?',
            'Банкет залы': 'Банкет залдары үлкен іс-шараларға өте ыңғайлы! Біз сізге орын таңдауға, орындықтарды орналастыруға немесе қосымша қызметтерді ұйымдастыруға көмектесе аламыз. Сізге не керек?',
            'Ашық алаң': 'Ашық алаңда іс-шара өткізу ерекше атмосфера сыйлайды! Біз сізге шатырларды, жарықтандыруды немесе кейтерингті ұйымдастыруға көмектесе аламыз. Сізге не қажет?',
            'Кафе': 'Кафеде жайлы іс-шара өткізуге болады! Біз сізге орын таңдауға, мәзір құруға немесе музыкалық сүйемелдеуге көмектесе аламыз. Сізге не керек?',
            'Коворкинг кеңістігі': 'Коворкинг кеңістігі іскерлік кездесулерге өте ыңғайлы! Біз сізге орынды брондауға, техникалық жабдықтарды ұйымдастыруға немесе кофе-брейктерді жоспарлауға көмектесе аламыз. Сізге не қажет?',
            'Балалар': 'Балаларға арналған іс-шаралар өте қызықты болуы мүмкін! Аниматорлар, тақырыптық кештер немесе ойын алаңдарын ұсына аламыз. Сізге қандай формат ұнайды?',
            'Жастар': 'Жастарға арналған іс-шаралар энергияға толы болады! Квесттер, музыкалық кештер немесе спорттық шараларды ұсына аламыз. Сізге не қызық?',
            'Ересектер': 'Ересектерге арналған іс-шаралар әртүрлі болуы мүмкін! Вечниверситеттік кездесулер немесе гастрономиялық кештерді ұйымдастыра аламыз. Сізге не керек?',
            'Қарттар': 'Қарттарға арналған іс-шаралар жайлы әрі қызықты болуы керек! Чай кештерін, музыкалық кештерді немесе серуендерді ұсына аламыз. Сізге қандай формат ұнайды?',
            'Барлық жас': 'Барлық жастағыларға арналған іс-шаралар отбасылық мерекелерге өте ыңғайлы! Фестивальдар, пикниктер немесе тақырыптық кештерді ұйымдастыра аламыз. Сізге не қызық?',
            'Танымал мейрамханалар': 'Танымал мейрамханаларда іс-шара өткізу – үлкен тәжірибе! Біз сізге ең жақсы орындарды таңдауға және брондауға көмектесеміз. Қандай мейрамхана стилі сізге ұнайды?',
            'Танымал іс-шаралар': 'Танымал іс-шаралар әрқашан есте қалады! Фестивальдар, концерттер немесе көрмелер туралы ақпарат бере аламыз. Сізге қандай іс-шара қызық?',
            'Танымал ашық алаңдар': 'Танымал ашық алаңдар ерекше іс-шараларға өте ыңғайлы! Біз сізге ең жақсы орындарды таңдауға және қосымша қызметтерді ұйымдастыруға көмектесеміз. Сізге не қажет?',
            'Танымал банкет залдары': 'Танымал банкет залдары үлкен іс-шараларға өте ыңғайлы! Біз сізге ең жақсы залдарды таңдауға және қосымша қызметтерді ұйымдастыруға көмектесеміз. Сізге қандай формат керек?',
            'Танымал кафе': 'Танымал кафеде жайлы іс-шара өткізуге болады! Біз сізге ең жақсы орындарды таңдауға және қосымша қызметтерді ұйымдастыруға көмектесеміз. Сізге қандай формат ұнайды?'
        },
        'ru': {
            'День рождения': 'Для организации дня рождения у нас есть несколько отличных идей! Например, тематические вечеринки, аниматоры или необычные локации. Какой формат вам интересен?',
            'Свадьба': 'Свадьба – это особенное событие! Мы можем помочь с подбором банкетных залов, декораторов или фотографов. Какие услуги вам нужны?',
            'Корпоратив': 'Корпоративные мероприятия объединяют вашу команду! Мы предлагаем идеи для тимбилдинга, кейтеринг или развлекательные программы. Какой формат вам нужен?',
            'Деловые встречи': 'Для деловых встреч мы предлагаем конференц-залы, техническое оборудование и кофе-брейки. Какие услуги вам нужны?',
            'Юбилей': 'Юбилей – это важный праздник! Мы можем помочь с подбором банкетных BUDDY залов, музыкальных программ или сюрпризов. Какой формат вам интересен?',
            'Ресторан': 'Проведение мероприятия в ресторане – отличный выбор! Мы можем помочь с бронированием столов, выбором меню или специальным декором. Что вам нужно?',
            'Банкетный зал': 'Банкетные залы идеально подходят для больших мероприятий! Мы можем помочь с выбором зала, рассадкой гостей или дополнительными услугами. Что вам нужно?',
            'Открытая площадка': 'Мероприятие на открытой площадке создаёт особую атмосферу! Мы можем помочь с организацией шатров, освещения или кейтеринга. Что вам нужно?',
            'Кафе': 'В кафе можно провести уютное мероприятие! Мы можем помочь с выбором места, составлением меню или музыкальным сопровождением. Что вам нужно?',
            'Коворкинг-пространство': 'Коворкинг-пространство идеально для деловых встреч! Мы можем помочь с бронированием места, техническим оборудованием или организацией кофе-брейков. Что вам нужно?',
            'Дети': 'Мероприятия для детей могут быть очень увлекательными! Мы предлагаем аниматоров, тематические вечеринки или игровые площадки. Какой формат вам интересен?',
            'Молодёжь': 'Мероприятия для молодёжи полны энергии! Мы предлагаем квесты, музыкальные вечеринки или спортивные мероприятия. Что вам интересно?',
            'Взрослые': 'Мероприятия для взрослых могут быть разнообразными! Мы можем организовать вечеринки, мастер-классы или гастрономические вечера. Что вам нужно?',
            'Пожилые': 'Мероприятия для пожилых должны быть уютными и интересными! Мы предлагаем чайные вечера, музыкальные встречи или прогулки. Какой формат вам интересен?',
            'Все возрасты': 'Мероприятия для всех возрастов идеальны для семейных праздников! Мы предлагаем фестивали, пикники или тематические вечеринки. Что вам интересно?',
            'Популярные рестораны': 'Проведение мероприятия в популярном ресторане – это незабываемо! Мы поможем выбрать и забронировать лучшие места. Какой стиль ресторана вам нравится?',
            'Популярные мероприятия': 'Популярные мероприятия всегда запоминаются! Мы можем рассказать о фестивалях, концертах или выставках. Какое мероприятие вам интересно?',
            'Популярные открытые площадки': 'Популярные открытые площадки идеальны для уникальных мероприятий! Мы поможем выбрать лучшее место и организовать дополнительные услуги. Что вам нужно?',
            'Популярные банкетные залы': 'Популярные банкетные залы идеальны для больших мероприятий! Мы поможем выбрать лучший зал и организовать дополнительные услуги. Какой формат вам нужен?',
            'Популярные кафе': 'В популярном кафе можно провести уютное мероприятие! Мы поможем выбрать лучшее место и организовать дополнительные услуги. Какой формат вам интересен?'
        },
        'en': {
            'Birthday': 'We have several great ideas for organizing a birthday! For example, themed parties, animators, or unique venues. What format are you interested in?',
            'Wedding': 'A wedding is a special event! We can help you find banquet halls, decorators, or photographers. What services do you need?',
            'Corporate': 'Corporate events bring your team together! We offer team-building ideas, catering, or entertainment programs. What format do you need?',
            'Business Meetings': 'For business meetings, we offer conference rooms, technical equipment, and coffee breaks. What services do you need?',
            'Anniversary': 'An anniversary is an important celebration! We can help with banquet halls, music programs, or surprises. What format are you interested in?',
            'Restaurant': 'Hosting an event at a restaurant is a great choice! We can help with table reservations, menu selection, or special decor. What do you need?',
            'Banquet Hall': 'Banquet halls are perfect for large events! We can help with venue selection, seating arrangements, or additional services. What do you need?',
            'Open Area': 'An event in an open area creates a unique atmosphere! We can help with tents, lighting, or catering. What do you need?',
            'Cafe': 'You can host a cozy event at a cafe! We can help with venue selection, menu planning, or music arrangements. What do you need?',
            'Coworking Space': 'A coworking space is ideal for business meetings! We can help with booking the space, technical equipment, or coffee breaks. What do you need?',
            'Children': 'Events for children can be very exciting! We offer animators, themed parties, or playgrounds. What format are you interested in?',
            'Youth': 'Events for youth are full of energy! We offer quests, music parties, or sports events. What are you interested in?',
            'Adults': 'Events for adults can be diverse! We can organize parties, workshops, or gastronomic evenings. What do you need?',
            'Seniors': 'Events for seniors should be cozy and engaging! We offer tea evenings, music gatherings, or walks. What format are you interested in?',
            'All Ages': 'Events for all ages are perfect for family celebrations! We offer festivals, picnics, or themed parties. What are you interested in?',
            'Popular Restaurants': 'Hosting an event at a popular restaurant is unforgettable! We can help you choose and book the best places. What restaurant style do you like?',
            'Popular Events': 'Popular events are always memorable! We can tell you about festivals, concerts, or exhibitions. Which event are you interested in?',
            'Popular Open Areas': 'Popular open areas are perfect for unique events! We can help you choose the best spot and arrange additional services. What do you need?',
            'Popular Banquet Halls': 'Popular banquet halls are ideal for large events! We can help you choose the best hall and arrange additional services. What format do you need?',
            'Popular Cafes': 'You can host a cozy event at a popular cafe! We can help you choose the best spot and arrange additional services. What format are you interested in?'
        }
    };

    const responses = subTopicResponses[selectedLanguage] || subTopicResponses['ru'];
    const responseText = responses[subTopic] || 'Извините, не могу ответить на этот вопрос.';

    const responseContainer = document.createElement('div');
    responseContainer.classList.add('message-container');
    const responseElement = document.createElement('div');
    responseElement.classList.add('message', 'received');
    responseElement.textContent = responseText;
    responseContainer.appendChild(responseElement);
    chatMessages.appendChild(responseContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    savedMessages.push({ text: responseText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    askIfHelped();

    startInactivityTimer();
}

// Ask if the bot helped
function askIfHelped() {
    const helpPrompts = {
        'kz': 'Мен сізге көмектесе алдым ба?',
        'ru': 'Я смог вам помочь?',
        'en': 'Was I able to help you?'
    };
    const promptText = helpPrompts[selectedLanguage] || helpPrompts['ru'];

    const messageContainer = document.createElement('div');
    messageContainer.classList.add('message-container');
    const messageElement = document.createElement('div');
    messageElement.classList.add('message', 'received');
    messageElement.textContent = promptText;

    const feedbackDiv = document.createElement('div');
    feedbackDiv.classList.add('topic-selection');
    feedbackDiv.id = 'feedback-selection';

    messageContainer.appendChild(messageElement);
    messageContainer.appendChild(feedbackDiv);
    chatMessages.appendChild(messageContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: promptText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    const feedbackTemplate = document.getElementById(`feedback-template-${selectedLanguage}`);
    if (feedbackTemplate) {
        const feedbackContent = feedbackTemplate.content.cloneNode(true);
        feedbackDiv.appendChild(feedbackContent);
    }
}

// Handle feedback selection (called from HTML)
function selectFeedback(feedback) {
    const feedbackContainer = document.createElement('div');
    feedbackContainer.classList.add('message-container');
    const feedbackElement = document.createElement('div');
    feedbackElement.classList.add('message');
    feedbackElement.textContent = feedback;
    feedbackContainer.appendChild(feedbackElement);
    chatMessages.appendChild(feedbackContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
    savedMessages.push({ text: feedback, type: 'sent' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    const responseText = feedback === (selectedLanguage === 'kz' ? 'Ия' : selectedLanguage === 'ru' ? 'Да' : 'Yes')
        ? (selectedLanguage === 'kz' ? 'Тамаша! Тағы сұрақтарыңыз болса, жазыңыз.' :
            selectedLanguage === 'ru' ? 'Отлично! Если есть ещё вопросы, пишите.' :
                'Great! If you have more questions, feel free to ask.')
        : (selectedLanguage === 'kz' ? 'Кешіріңіз, тағы қалай көмектесе аламын?' :
            selectedLanguage === 'ru' ? 'Извините, чем ещё могу помочь?' :
                'Sorry, how else can I assist you?');

    const responseContainer = document.createElement('div');
    responseContainer.classList.add('message-container');
    const responseElement = document.createElement('div');
    responseElement.classList.add('message', 'received');
    responseElement.textContent = responseText;
    responseContainer.appendChild(responseElement);
    chatMessages.appendChild(responseContainer);
    chatMessages.scrollTop = chatMessages.scrollHeight;

    savedMessages.push({ text: responseText, type: 'received' });
    localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

    showTopicSelection();

    startInactivityTimer();
}






// Fetch AI response for Assistant Bot
async function getAIResponse(message) {
    try {
        const response = await fetch('https://openrouter.ai/api/v1/chat/completions', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer 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',
                'HTTP-Referer': 'https://your-site.com',  // Замените при необходимости
                'X-Title': 'Your App Name'
            },
            body: JSON.stringify({
                model: "meta-llama/llama-3.1-8b-instruct:free",
                messages: [
                    { role: "user", content: message }
                ],
                max_tokens: 150
            })
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`HTTP error! Status: ${response.status}, Body: ${errorText}`);
        }

        const data = await response.json();
        return data.choices?.[0]?.message?.content ?? 'Не удалось получить ответ от API.';
    } catch (error) {
        return 'Не удалось получить ответ от сервера: ' + error.message;
    }
}










// Start inactivity timer (10 minutes)
function startInactivityTimer() {
    if (inactivityTimer) {
        clearTimeout(inactivityTimer);
    }

    inactivityTimer = setTimeout(() => {
        if (selectedTopic === (selectedLanguage === 'kz' ? 'Ассистент бот' : selectedLanguage === 'ru' ? 'Ассистент бот' : 'Assistant Bot')) {
            askIfHelped();
        }
    }, 600000);
}

// Send message
async function sendMessage() {
    const message = messageInput.value.trim();
    if (message) {
        const now = new Date();
        let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];

        if (!lastTimeDisplayed || now - lastTimeDisplayed >= 86400000) {
            const timeElement = document.createElement('div');
            timeElement.classList.add('message-time');
            timeElement.textContent = now.toLocaleDateString();
            chatMessages.appendChild(timeElement);
            lastTimeDisplayed = now;
        }

        const messageContainer = document.createElement('div');
        messageContainer.classList.add('message-container');
        const messageElement = document.createElement('div');
        messageElement.classList.add('message');
        messageElement.textContent = message;
        messageContainer.appendChild(messageElement);
        chatMessages.appendChild(messageContainer);

        savedMessages.push({ text: message, type: 'sent' });
        localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

        messageInput.value = '';
        sendBtn.style.display = 'none';
        chatMessages.scrollTop = chatMessages.scrollHeight;

        let responseText;
        const topics = {
            'kz': [
                'Іс-шара түрі',
                'Орын түрі',
                'Аудитория бойынша',
                'Танымал',
                'Ассистент бот'
            ],
            'ru': [
                'Тип мероприятия',
                'Тип места',
                'По аудитории',
                'Популярное',
                'Ассистент бот'
            ],
            'en': [
                'Event Type',
                'Place Type',
                'By Audience',
                'Popular',
                'Assistant Bot'
            ]
        };
        const topicList = topics[selectedLanguage] || topics['ru'];

        if (selectedTopic === (selectedLanguage === 'kz' ? 'Ассистент бот' : selectedLanguage === 'ru' ? 'Ассистент бот' : 'Assistant Bot')) {
            const typingContainer = document.createElement('div');
            typingContainer.classList.add('assistant-message-container');

            const header = document.createElement('div');
            header.classList.add('assistant-header');
            const name = document.createElement('span');
            name.classList.add('assistant-name');
            name.textContent = 'VibePlace Bot';
            header.appendChild(name);

            const typingElement = document.createElement('div');
            typingElement.classList.add('typing-indicator');
            typingElement.innerHTML = '<span></span><span></span><span></span>';
            typingElement.style.display = 'block';

            typingContainer.appendChild(header);
            typingContainer.appendChild(typingElement);
            chatMessages.appendChild(typingContainer);
            chatMessages.scrollTop = chatMessages.scrollHeight;

            await new Promise(resolve => setTimeout(resolve, 1000));
            chatMessages.removeChild(typingContainer);

            responseText = await getAIResponse(message);

            const assistantContainer = document.createElement('div');
            assistantContainer.classList.add('assistant-message-container');

            const assistantHeader = document.createElement('div');
            assistantHeader.classList.add('assistant-header');
            const assistantName = document.createElement('span');
            assistantName.classList.add('assistant-name');
            assistantName.textContent = 'VibePlace Bot';
            assistantHeader.appendChild(assistantName);

            const messageWrapper = document.createElement('div');
            messageWrapper.classList.add('assistant-message-wrapper');
            const avatar = document.createElement('div');
            avatar.classList.add('assistant-avatar');
            avatar.textContent = 'VP';
            const responseElement = document.createElement('div');
            responseElement.classList.add('assistant-message');
            responseElement.textContent = responseText;
            messageWrapper.appendChild(avatar);
            messageWrapper.appendChild(responseElement);

            assistantContainer.appendChild(assistantHeader);
            assistantContainer.appendChild(messageWrapper);
            chatMessages.appendChild(assistantContainer);
            chatMessages.scrollTop = chatMessages.scrollHeight;

            savedMessages.push({ text: responseText, type: 'received' });
            localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
        } else {
            showTopicSelection();
        }

        while (savedMessages.length > 50) {
            savedMessages.shift();
        }
        localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
        while (chatMessages.children.length > 50) {
            chatMessages.removeChild(chatMessages.firstChild);
        }

        startInactivityTimer();
    }
}

// Handle file upload
attachBtn.addEventListener('click', () => {
    fileInput.click();
});

fileInput.addEventListener('change', () => {
    const file = fileInput.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = (e) => {
            const messageContainer = document.createElement('div');
            messageContainer.classList.add('message-container');
            const messageElement = document.createElement('div');
            messageElement.classList.add('message');
            const img = document.createElement('img');
            img.src = e.target.result;
            messageElement.appendChild(img);
            messageContainer.appendChild(messageElement);
            chatMessages.appendChild(messageContainer);
            chatMessages.scrollTop = chatMessages.scrollHeight;

            let savedMessages = JSON.parse(localStorage.getItem('chatMessages')) || [];
            savedMessages.push({ text: e.target.result, type: 'sent', isImage: true });
            localStorage.setItem('chatMessages', JSON.stringify(savedMessages));

            while (savedMessages.length > 50) {
                savedMessages.shift();
            }
            localStorage.setItem('chatMessages', JSON.stringify(savedMessages));
            while (chatMessages.children.length > 50) {
                chatMessages.removeChild(chatMessages.firstChild);
            }

            startInactivityTimer();
        };
        reader.readAsDataURL(file);
    }
});

// Send message on button click or Enter
sendBtn.addEventListener('click', sendMessage);
messageInput.addEventListener('keypress', (e) => {
    if (e.key === 'Enter') sendMessage();
});

// Toggle chat visibility
chatToggle.addEventListener('click', () => {
    console.log('Chat toggle clicked');
    const isChatVisible = chatContainer.style.display === 'flex';
    chatContainer.style.display = isChatVisible ? 'none' : 'flex';
    if (!isChatVisible && !selectedLanguage) {
        console.log('Chat opened, showing language selection');
        showLanguageSelection();
    }
});