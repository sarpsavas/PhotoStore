/* ═══════════════════════════════════════
   PHOTOSTORE — app.js
   ═══════════════════════════════════════ */

const API = 'http://localhost:5100/api/v1';

const CAT = {
  1:'Kamera', 11:'Dijital Kamera', 12:'Analog Kamera',
  2:'Objektif',
  3:'Film', 31:'35mm', 32:'120mm',
  4:'Aksesuar', 41:'Tripod', 42:'Kamera Çantası', 43:'Hafıza Kartı',
  44:'Batarya & Şarj', 45:'Flaş & Işık', 46:'Gimbal & Stabilizer',
  5:'Filtre',
};

const CAT_CHILDREN = {
  1:[1,11,12], 3:[3,31,32],
  4:[4,41,42,43,44,45,46],
};

const TX_TYPE = {
  0:'Kayıt', 1:'İlan Eklendi', 2:'İlan Güncellendi', 3:'İlan Silindi',
  4:'Soru Eklendi', 5:'Cevap Eklendi', 6:'Profil Güncellendi', 7:'Fotoğraf Eklendi',
};

/* ── State ─────────────────────────────── */
let state = {
  user:            null,   // { id, email, token }
  listings:        [],
  questions:       [],
  pageNum:         0,
  filterCat:       0,
  detailListing:   null,
  detailQuestions: [],
  detailImages:    [],
  galleryIndex:    0,
};

/* ══════════════════════════════════════
   API HELPER
══════════════════════════════════════ */

function authHeaders(extra = {}) {
  const h = { 'Content-Type': 'application/json', ...extra };
  if (state.user?.token) h['Authorization'] = `Bearer ${state.user.token}`;
  return h;
}

async function api(path, opts = {}) {
  const res = await fetch(API + path, {
    headers: authHeaders(),
    ...opts,
  });
  if (res.status === 401) {
    toast('Oturum süresi doldu. Lütfen tekrar giriş yapın.', 'error');
    setUser(null);
    throw new Error('Unauthorized');
  }
  if (!res.ok) {
    const txt = await res.text().catch(() => '');
    throw new Error(txt || res.statusText);
  }
  const txt = await res.text();
  return txt ? JSON.parse(txt) : null;
}

/* multipart/form-data upload — no Content-Type header (browser sets boundary) */
async function apiUpload(path, formData) {
  const headers = {};
  if (state.user?.token) headers['Authorization'] = `Bearer ${state.user.token}`;
  const res = await fetch(API + path, { method: 'POST', headers, body: formData });
  if (res.status === 401) { toast('Oturum süresi doldu.', 'error'); setUser(null); throw new Error('Unauthorized'); }
  if (!res.ok) { const t = await res.text().catch(() => ''); throw new Error(t || res.statusText); }
  const txt = await res.text();
  return txt ? JSON.parse(txt) : null;
}

/* ══════════════════════════════════════
   TOAST
══════════════════════════════════════ */

function toast(msg, type = 'info') {
  const c = document.getElementById('toast-container');
  const el = document.createElement('div');
  el.className = `toast ${type}`;
  el.textContent = msg;
  c.appendChild(el);
  setTimeout(() => {
    el.style.transition = 'opacity .3s, transform .3s';
    el.style.opacity = '0';
    el.style.transform = 'translateX(10px)';
    setTimeout(() => el.remove(), 300);
  }, 3200);
}

/* ══════════════════════════════════════
   NAVIGATION
══════════════════════════════════════ */

function navigate(page) {
  if (page === 'mylistings' && !state.user) {
    toast('Giriş yapmanız gerekiyor.', 'info');
    openModal('modal-login');
    return;
  }
  document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
  document.querySelectorAll('.nav-btn').forEach(b => b.classList.toggle('active', b.dataset.page === page));
  document.getElementById('page-' + page).classList.add('active');
  if (page === 'mylistings') loadMyListings();
}

/* ══════════════════════════════════════
   AUTH
══════════════════════════════════════ */

function setUser(user) {
  state.user = user;
  const inEl  = document.getElementById('ua-in');
  const outEl = document.getElementById('ua-out');
  const chip  = document.getElementById('user-chip');
  if (user) {
    inEl.style.display  = 'flex';
    outEl.style.display = 'none';
    chip.textContent = user.email;
  } else {
    inEl.style.display  = 'none';
    outEl.style.display = 'flex';
    chip.textContent = '';
  }
}

function logout() {
  setUser(null);
  navigate('home');
  toast('Çıkış yapıldı.', 'info');
}

async function doLogin() {
  const email = document.getElementById('login-email').value.trim();
  const pass  = document.getElementById('login-pass').value;
  if (!email || !pass) { toast('E-posta ve şifre gerekli.', 'info'); return; }
  try {
    /* Returns { userId, token } */
    const res = await fetch(`${API}/User/user/log-in?EMail=${encodeURIComponent(email)}&Password=${encodeURIComponent(pass)}`, {
      headers: { 'Content-Type': 'application/json' },
    });
    if (!res.ok) throw new Error();
    const data = await res.json();
    setUser({ id: data.userId, email, token: data.token });
    closeModal('modal-login');
    toast('Hoş geldiniz!', 'success');
  } catch { toast('Giriş başarısız. Bilgileri kontrol edin.', 'error'); }
}

async function doRegister() {
  const name     = document.getElementById('reg-name').value.trim();
  const lastName = document.getElementById('reg-lastname').value.trim();
  const email    = document.getElementById('reg-email').value.trim();
  const password = document.getElementById('reg-pass').value;
  if (!name || !lastName || !email || !password) { toast('Tüm alanları doldurun.', 'info'); return; }
  try {
    await api('/User/user/register', {
      method: 'POST',
      body: JSON.stringify({ name, lastName, eMail: email, password }),
    });
    closeModal('modal-register');
    toast('Kayıt başarılı. Giriş yapabilirsiniz.', 'success');
  } catch { toast('Kayıt başarısız.', 'error'); }
}

/* ══════════════════════════════════════
   MODAL HELPERS
══════════════════════════════════════ */

function openModal(id) {
  document.getElementById(id).classList.add('open');
  if (id === 'modal-transactions') loadTransactions();
}
function closeModal(id) {
  document.getElementById(id).classList.remove('open');
}
function closeModalOverlay(e, id) {
  if (e.target === document.getElementById(id)) closeModal(id);
}
function switchTab(btn, tabId) {
  const modal = btn.closest('.modal');
  modal.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
  modal.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
  btn.classList.add('active');
  document.getElementById(tabId).classList.add('active');
}

/* ══════════════════════════════════════
   LISTINGS — LOAD & RENDER
══════════════════════════════════════ */

async function loadListings(page = 0) {
  showLoading(true);
  try {
    const data = await api(`/Listing/listing/all-listings?page=${page}`);
    state.listings  = data.listings  || [];
    state.questions = data.questions || [];
    state.pageNum   = page;
    renderListings();
    updatePagination();
  } catch (e) {
    if (e.message !== 'Unauthorized') toast('İlanlar yüklenemedi.', 'error');
  }
  showLoading(false);
}

function showLoading(show) {
  document.getElementById('listings-loading').style.display = show ? 'flex' : 'none';
  document.getElementById('listings-grid').style.display    = show ? 'none' : 'grid';
}

function renderListings() {
  const grid       = document.getElementById('listings-grid');
  const empty      = document.getElementById('listings-empty');
  const countEl    = document.getElementById('listing-count');
  const filterLabel= document.getElementById('active-filter-label');

  const search   = document.getElementById('search-input').value.toLowerCase().trim();
  const priceMin = parseFloat(document.getElementById('price-min').value) || 0;
  const priceMax = parseFloat(document.getElementById('price-max').value) || Infinity;
  const cat      = state.filterCat;

  let allowedCats = null;
  if (cat !== 0) allowedCats = new Set(CAT_CHILDREN[cat] || [cat]);

  const filtered = state.listings.filter(l => {
    const catOk   = !allowedCats || allowedCats.has(l.category);
    const srcOk   = !search ||
      (l.listingName || '').toLowerCase().includes(search) ||
      (l.address     || '').toLowerCase().includes(search) ||
      (l.userName    || '').toLowerCase().includes(search);
    const priceOk = l.price >= priceMin && l.price <= priceMax;
    return catOk && srcOk && priceOk;
  });

  countEl.textContent = `${filtered.length} ilan`;
  if (cat !== 0) {
    filterLabel.textContent = CAT[cat] || '';
    filterLabel.style.display = 'inline-flex';
  } else {
    filterLabel.style.display = 'none';
  }

  if (!filtered.length) { grid.innerHTML = ''; empty.style.display = 'flex'; return; }
  empty.style.display = 'none';

  grid.innerHTML = filtered.map(l => {
    const idx = state.listings.findIndex(x => x.listingId === l.listingId);
    const qs  = state.questions[idx] || [];
    return cardHTML(l, qs);
  }).join('');
}

function cardHTML(l, qs) {
  const catName  = CAT[l.category] || 'Diğer';
  const qBadge   = qs.length ? `<div class="card-q-badge">${qs.length}</div>` : '';
  const imgPart  = l.imageUrls && l.imageUrls.length
    ? `<img class="card-img-photo" src="${esc(l.imageUrls[0])}" alt="${esc(l.listingName)}" loading="lazy" />`
    : `<div class="card-img-placeholder"><svg width="44" height="44" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1" stroke-linecap="round" stroke-linejoin="round"><path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/><circle cx="12" cy="13" r="4"/></svg></div>`;
  const lJ = escJson(l);
  const qJ = escJson(qs);

  return `
    <div class="listing-card" onclick='openDetail(${lJ},${qJ})'>
      <div class="card-img">
        ${imgPart}
        <div class="card-badges"><span class="badge badge-${l.category || 5}">${esc(catName)}</span></div>
        ${qBadge}
      </div>
      <div class="card-body">
        <div class="card-title">${esc(l.listingName)}</div>
        <div class="card-meta">
          <span class="card-meta-item">
            <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
            ${esc(l.userName || '—')}
          </span>
          <span class="card-meta-item">
            <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/></svg>
            ${esc(l.address || '—')}
          </span>
        </div>
        <div class="card-price">₺${Number(l.price).toLocaleString('tr-TR')}</div>
      </div>
    </div>`;
}

function filterListings() { renderListings(); }

function setCat(val, btn) {
  state.filterCat = val;
  document.querySelectorAll('.filter-parent, .filter-child').forEach(b => b.classList.remove('active'));
  btn.classList.add('active');
  renderListings();
}

function changePage(dir) {
  const next = state.pageNum + dir;
  if (next < 0) return;
  loadListings(next);
  window.scrollTo({ top: 0, behavior: 'smooth' });
}

function updatePagination() {
  document.getElementById('page-label').textContent = state.pageNum + 1;
  document.getElementById('btn-prev').disabled      = state.pageNum === 0;
  document.getElementById('btn-next').disabled      = state.listings.length < 10;
}

/* ══════════════════════════════════════
   DETAIL MODAL
══════════════════════════════════════ */

async function openDetail(listing, qs) {
  state.detailListing   = listing;
  state.detailQuestions = qs;
  state.detailImages    = [];
  state.galleryIndex    = 0;
  renderDetail();
  openModal('modal-detail');
  /* Load images */
  try {
    const imgs = await api('/Listing/listing/get-image', {
      method: 'PATCH',
      body: JSON.stringify({ listingId: listing.listingId }),
    });
    state.detailImages = imgs || [];
  } catch { state.detailImages = listing.imageUrls || []; }
  renderGallery();
}

function renderDetail() {
  const l       = state.detailListing;
  const qs      = state.detailQuestions;
  const isOwner = state.user && l.userId === state.user.id;

  document.getElementById('detail-title').textContent = l.listingName;

  document.getElementById('detail-meta').innerHTML = `
    <span class="badge badge-${l.category || 5}">${esc(CAT[l.category] || 'Diğer')}</span>
    <span class="meta-chip">
      <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>
      ${esc(l.userName)}
    </span>
    <span class="meta-chip">
      <svg width="11" height="11" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/></svg>
      ${esc(l.address)}
    </span>
    <span class="meta-chip">${new Date(l.listingDate).toLocaleDateString('tr-TR')}</span>
  `;

  document.getElementById('detail-stats').innerHTML = `
    <div class="stat-box">
      <div class="stat-label">Fiyat</div>
      <div class="stat-value price">₺${Number(l.price).toLocaleString('tr-TR')}</div>
    </div>
    <div class="stat-box">
      <div class="stat-label">İletişim</div>
      <div class="stat-value" style="font-size:16px">${esc(l.contact)}</div>
    </div>
  `;

  const descWrap = document.getElementById('detail-desc-wrap');
  if (l.listingDescription) {
    descWrap.style.display = 'block';
    document.getElementById('detail-desc').textContent = l.listingDescription;
  } else {
    descWrap.style.display = 'none';
  }

  const qBadge = document.getElementById('detail-q-count');
  qBadge.textContent     = qs.length || '';
  qBadge.style.display   = qs.length ? 'inline-flex' : 'none';

  renderQuestions(qs, isOwner);

  /* Ask / hint */
  const askWrap = document.getElementById('ask-question-wrap');
  const askHint = document.getElementById('ask-question-hint');
  if (state.user && !isOwner) { askWrap.style.display='flex'; askHint.style.display='none'; }
  else if (!state.user)       { askWrap.style.display='none'; askHint.style.display='block'; }
  else                         { askWrap.style.display='none'; askHint.style.display='none'; }

  document.getElementById('question-input').value = '';

  /* Upload button */
  document.getElementById('detail-img-upload-wrap').style.display = isOwner ? 'block' : 'none';
}

/* ── Gallery ── */
function renderGallery() {
  const inner = document.getElementById('detail-gallery-inner');
  const imgs  = state.detailImages;

  if (!imgs.length) {
    inner.innerHTML = `
      <div class="gallery-placeholder">
        <svg width="56" height="56" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width=".8" stroke-linecap="round" stroke-linejoin="round">
          <path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/>
          <circle cx="12" cy="13" r="4"/>
        </svg>
      </div>`;
    renderDots(0);
    return;
  }

  inner.innerHTML = imgs.map(url =>
    `<div class="gallery-slide"><img src="${esc(url)}" alt="İlan fotoğrafı" loading="lazy" /></div>`
  ).join('');

  /* scroll to current */
  const slides = inner.querySelectorAll('.gallery-slide');
  if (slides[state.galleryIndex]) slides[state.galleryIndex].scrollIntoView({ behavior:'auto', inline:'start' });

  renderDots(imgs.length);
}

function renderDots(count) {
  const gallery = document.getElementById('detail-gallery');
  let dots = gallery.querySelector('.gallery-dots');
  if (dots) dots.remove();
  if (count < 2) return;
  dots = document.createElement('div');
  dots.className = 'gallery-dots';
  for (let i = 0; i < count; i++) {
    const d = document.createElement('div');
    d.className = 'gallery-dot' + (i === state.galleryIndex ? ' active' : '');
    d.onclick = () => scrollGallery(i);
    dots.appendChild(d);
  }
  gallery.appendChild(dots);
}

function scrollGallery(idx) {
  state.galleryIndex = idx;
  const inner  = document.getElementById('detail-gallery-inner');
  const slides = inner.querySelectorAll('.gallery-slide');
  if (slides[idx]) slides[idx].scrollIntoView({ behavior:'smooth', inline:'start' });
  inner.closest('#detail-gallery').querySelectorAll('.gallery-dot').forEach((d, i) => {
    d.classList.toggle('active', i === idx);
  });
}

/* ── Image upload ── */
async function uploadImage(input) {
  if (!input.files || !input.files[0]) return;
  if (!state.user || !state.detailListing) return;
  const file = input.files[0];
  const fd   = new FormData();
  fd.append('file', file);
  /* listingId and userId as query params */
  const path = `/Listing/listing/add-image?listingId=${state.detailListing.listingId}&userId=${state.user.id}`;
  try {
    toast('Fotoğraf yükleniyor...', 'info');
    const urls = await apiUpload(path, fd);
    state.detailImages = Array.isArray(urls) ? urls : state.detailImages;
    renderGallery();
    toast('Fotoğraf eklendi.', 'success');
  } catch { toast('Fotoğraf yüklenemedi.', 'error'); }
  input.value = '';
}

/* ── Questions ── */
function renderQuestions(qs, isOwner) {
  const el = document.getElementById('detail-questions');
  if (!qs.length) { el.innerHTML = '<p class="hint-text">Henüz soru yok.</p>'; return; }
  el.innerHTML = qs.map(q => {
    let answerPart = '';
    if (q.answerText) {
      answerPart = `<div class="answer-wrap">${esc(q.answerText)}</div>`;
    } else if (isOwner) {
      answerPart = `
        <div class="answer-input-wrap">
          <input class="input" id="ans-${esc(q.questionId)}" placeholder="Cevabınız..." />
          <button class="btn-primary" style="padding:7px 13px;font-size:12px" onclick="submitAnswer('${esc(q.questionId)}')">Gönder</button>
        </div>`;
    } else {
      answerPart = `<div class="answer-pending">Cevaplanmadı</div>`;
    }
    return `
      <div class="question-item" id="qi-${esc(q.questionId)}">
        <div class="q-text"><span class="q-dot"></span>${esc(q.questionText)}</div>
        ${answerPart}
      </div>`;
  }).join('');
}

async function submitQuestion() {
  if (!state.user || !state.detailListing) return;
  const input = document.getElementById('question-input');
  const text  = input.value.trim();
  if (!text) return;
  try {
    await api('/Listing/listing/add-question', {
      method: 'POST',
      body: JSON.stringify({ userId: state.user.id, questionText: text, listingId: state.detailListing.listingId }),
    });
    const newQ = {
      questionId: Date.now().toString(), userId: state.user.id,
      questionText: text, listingId: state.detailListing.listingId,
      answerText: null, questionDate: new Date().toISOString(), answerDate: null,
    };
    state.detailQuestions.push(newQ);
    renderQuestions(state.detailQuestions, false);
    const badge = document.getElementById('detail-q-count');
    badge.textContent = state.detailQuestions.length;
    badge.style.display = 'inline-flex';
    input.value = '';
    toast('Soru gönderildi.', 'success');
  } catch (e) { if (e.message !== 'Unauthorized') toast('Soru gönderilemedi.', 'error'); }
}

async function submitAnswer(questionId) {
  if (!state.user) return;
  const input = document.getElementById('ans-' + questionId);
  if (!input) return;
  const text = input.value.trim();
  if (!text) return;
  try {
    await api('/Listing/listing/add-answer', {
      method: 'PATCH',
      body: JSON.stringify({ questionId, answerText: text, userId: state.user.id }),
    });
    const q = state.detailQuestions.find(x => x.questionId === questionId);
    if (q) { q.answerText = text; q.answerDate = new Date().toISOString(); }
    renderQuestions(state.detailQuestions, true);
    toast('Cevap gönderildi.', 'success');
  } catch (e) { if (e.message !== 'Unauthorized') toast('Cevap gönderilemedi.', 'error'); }
}

/* ══════════════════════════════════════
   LISTING FORM — ADD / EDIT
══════════════════════════════════════ */

function openAddListing() {
  if (!state.user) { toast('Giriş yapmanız gerekiyor.', 'info'); openModal('modal-login'); return; }
  document.getElementById('listing-form-title').textContent = 'İlan Ekle';
  document.getElementById('form-listing-id').value = '';
  ['form-name','form-desc','form-address','form-contact','form-price'].forEach(id => {
    document.getElementById(id).value = '';
  });
  document.getElementById('form-category').value = '1';
  openModal('modal-add-listing');
}

function openEditListing(listing) {
  document.getElementById('listing-form-title').textContent = 'İlanı Düzenle';
  document.getElementById('form-listing-id').value  = listing.listingId;
  document.getElementById('form-name').value        = listing.listingName        || '';
  document.getElementById('form-desc').value        = listing.listingDescription || '';
  document.getElementById('form-address').value     = listing.address            || '';
  document.getElementById('form-contact').value     = listing.contact            || '';
  document.getElementById('form-price').value       = listing.price              || '';
  document.getElementById('form-category').value    = String(listing.category    || 1);
  openModal('modal-add-listing');
}

async function submitListing() {
  if (!state.user) return;
  const id       = document.getElementById('form-listing-id').value;
  const name     = document.getElementById('form-name').value.trim();
  const desc     = document.getElementById('form-desc').value.trim();
  const address  = document.getElementById('form-address').value.trim();
  const contact  = document.getElementById('form-contact').value.trim();
  const price    = parseFloat(document.getElementById('form-price').value);
  const category = parseInt(document.getElementById('form-category').value);

  if (!name || !address || !contact || isNaN(price)) { toast('Lütfen zorunlu alanları doldurun.', 'info'); return; }

  try {
    if (id) {
      await api('/Listing/listing/update', {
        method: 'PATCH',
        body: JSON.stringify({ listingId: id, userId: state.user.id, listingName: name, listingDescription: desc, address, contact, price, category }),
      });
      toast('İlan güncellendi.', 'success');
    } else {
      await api('/Listing/listing/add', {
        method: 'POST',
        body: JSON.stringify({ userId: state.user.id, listingName: name, listingDescription: desc, address, contact, price, category, imageUrls: [] }),
      });
      toast('İlan eklendi.', 'success');
    }
    closeModal('modal-add-listing');
    loadListings(state.pageNum);
  } catch (e) { if (e.message !== 'Unauthorized') toast('İşlem başarısız.', 'error'); }
}

/* ══════════════════════════════════════
   MY LISTINGS
══════════════════════════════════════ */

async function loadMyListings() {
  if (!state.user) return;
  const list  = document.getElementById('my-listings-list');
  const empty = document.getElementById('my-listings-empty');
  list.innerHTML = '<div class="loading-state"><div class="spinner"></div></div>';
  empty.style.display = 'none';
  try {
    const data      = await api(`/Listing/listing/user-listings?UserId=${state.user.id}`);
    const listings  = data.listings  || [];
    const questions = data.questions || [];
    if (!listings.length) { list.innerHTML = ''; empty.style.display = 'flex'; return; }
    list.innerHTML = listings.map((l, i) => {
      const qs    = questions[i] || [];
      const lJ    = escJson(l);
      const thumb = l.imageUrls && l.imageUrls.length
        ? `<img src="${esc(l.imageUrls[0])}" alt="" />`
        : `<svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.4" stroke-linecap="round" stroke-linejoin="round"><path d="M23 19a2 2 0 0 1-2 2H3a2 2 0 0 1-2-2V8a2 2 0 0 1 2-2h4l2-3h6l2 3h4a2 2 0 0 1 2 2z"/><circle cx="12" cy="13" r="4"/></svg>`;
      return `
        <div class="my-listing-row">
          <div class="my-listing-thumb">${thumb}</div>
          <div class="my-listing-info">
            <div class="my-listing-name">${esc(l.listingName)}</div>
            <div class="my-listing-chips">
              <span class="badge badge-${l.category||5}" style="font-size:9px;padding:2px 8px">${esc(CAT[l.category]||'Diğer')}</span>
              <span class="chip">₺${Number(l.price).toLocaleString('tr-TR')}</span>
              <span class="chip">${qs.length} soru</span>
              <span class="chip">${new Date(l.listingDate).toLocaleDateString('tr-TR')}</span>
            </div>
          </div>
          <div class="my-listing-actions">
            <button class="btn-outline-sm" onclick='openEditListing(${lJ})'>Düzenle</button>
            <button class="btn-danger-sm"  onclick="deleteListing('${esc(l.listingId)}',this)">Sil</button>
          </div>
        </div>`;
    }).join('');
  } catch (e) { if (e.message !== 'Unauthorized') toast('İlanlar yüklenemedi.', 'error'); }
}

async function deleteListing(id, btn) {
  if (!confirm('Bu ilanı silmek istediğinizden emin misiniz?')) return;
  try {
    await api('/Listing/listing/delete', {
      method: 'DELETE',
      body: JSON.stringify({ listingId: id }),
    });
    btn.closest('.my-listing-row').remove();
    toast('İlan silindi.', 'success');
    loadListings(state.pageNum);
  } catch (e) { if (e.message !== 'Unauthorized') toast('Silme başarısız.', 'error'); }
}

/* ══════════════════════════════════════
   PROFILE
══════════════════════════════════════ */

async function updateName() {
  if (!state.user) return;
  const name     = document.getElementById('prof-name').value.trim();
  const lastName = document.getElementById('prof-lastname').value.trim();
  if (!name || !lastName) { toast('Ad ve soyad gerekli.', 'info'); return; }
  try {
    await api('/User/user/update-name', {
      method: 'PATCH',
      body: JSON.stringify({ userId: state.user.id, name, lastName }),
    });
    toast('İsim güncellendi.', 'success');
  } catch (e) { if (e.message !== 'Unauthorized') toast('Güncelleme başarısız.', 'error'); }
}

async function updateEmail() {
  if (!state.user) return;
  const email = document.getElementById('prof-email').value.trim();
  if (!email) { toast('E-posta gerekli.', 'info'); return; }
  try {
    await api('/User/user/update-email', {
      method: 'PATCH',
      body: JSON.stringify({ userId: state.user.id, eMail: email }),
    });
    setUser({ ...state.user, email });
    toast('E-posta güncellendi.', 'success');
  } catch (e) { if (e.message !== 'Unauthorized') toast('Güncelleme başarısız.', 'error'); }
}

async function updatePass() {
  if (!state.user) return;
  const oldPass = document.getElementById('prof-old-pass').value;
  const newPass = document.getElementById('prof-new-pass').value;
  if (!oldPass || !newPass) { toast('Her iki şifre alanı gerekli.', 'info'); return; }
  try {
    await api('/User/user/update-password', {
      method: 'PATCH',
      body: JSON.stringify({ userId: state.user.id, oldPasswordHash: oldPass, newPasswordHash: newPass }),
    });
    toast('Şifre güncellendi.', 'success');
  } catch (e) { if (e.message !== 'Unauthorized') toast('Güncelleme başarısız.', 'error'); }
}

/* ══════════════════════════════════════
   TRANSACTIONS
══════════════════════════════════════ */

async function loadTransactions() {
  if (!state.user) return;
  const el = document.getElementById('transactions-list');
  el.innerHTML = '<div class="loading-state"><div class="spinner"></div></div>';
  try {
    const txs = await api(`/Transaction/transaction/user-transactions?UserId=${state.user.id}`);
    if (!txs || !txs.length) { el.innerHTML = '<p class="hint-text">İşlem bulunamadı.</p>'; return; }
    el.innerHTML = txs.map(tx => `
      <div class="tx-row">
        <div class="tx-left">
          <div class="tx-dot" style="background:${tx.success===0?'var(--accent)':'#a099a5'}"></div>
          <div>
            <div class="tx-type">${esc(TX_TYPE[tx.type] || 'Bilinmeyen')}</div>
            <div class="tx-time">${new Date(tx.transactionTime).toLocaleString('tr-TR')}</div>
          </div>
        </div>
        <span class="tx-status ${tx.success===0?'ok':'fail'}">${tx.success===0?'Başarılı':'Başarısız'}</span>
      </div>`).join('');
  } catch (e) {
    if (e.message !== 'Unauthorized') el.innerHTML = '<p class="hint-text" style="color:var(--accent)">Yüklenemedi.</p>';
  }
}

/* ══════════════════════════════════════
   UTILS
══════════════════════════════════════ */

function escJson(obj) {
  return "'" + JSON.stringify(obj).replace(/\\/g,'\\\\').replace(/'/g,"\\'") + "'";
}
function esc(s) {
  return String(s ?? '').replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;').replace(/"/g,'&quot;');
}

/* ══════════════════════════════════════
   INIT
══════════════════════════════════════ */

document.addEventListener('DOMContentLoaded', () => {
  setUser(null);
  loadListings(0);
});
