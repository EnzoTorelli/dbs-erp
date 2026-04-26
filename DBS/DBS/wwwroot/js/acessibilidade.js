/**
 * acessibilidade.js — eMAG 3.1
 * Fonte (5 níveis) + Modo Escuro + Alto Contraste
 * Persiste em localStorage, respeita prefers-color-scheme
 */
(function () {
    'use strict';

    var HTML = document.documentElement;
    var FONTE_MIN = 1, FONTE_MAX = 4, FONTE_PADRAO = 2;
    var fonteAtual, escuroAtivo, contrasteAtivo;

    /* ── storage ── */
    function salvar(k, v) { try { localStorage.setItem('acess_' + k, v); } catch (_) {} }
    function carregar(k, pad) { try { return localStorage.getItem('acess_' + k) ?? pad; } catch (_) { return pad; } }

    /* ── live region para leitores de tela ── */
    function anunciar(msg) {
        var el = document.getElementById('acess-live');
        if (!el) {
            el = document.createElement('div');
            el.id = 'acess-live';
            el.setAttribute('aria-live', 'polite');
            el.setAttribute('aria-atomic', 'true');
            el.style.cssText = 'position:absolute;width:1px;height:1px;overflow:hidden;clip:rect(0,0,0,0);white-space:nowrap';
            document.body.appendChild(el);
        }
        el.textContent = '';
        requestAnimationFrame(function () { el.textContent = msg; });
    }

    /* ══ FONTE ══════════════════════════════════════════════════ */
    function aplicarFonte(nivel) {
        fonteAtual = Math.max(FONTE_MIN, Math.min(FONTE_MAX, nivel));
        HTML.setAttribute('data-fonte', fonteAtual);
        salvar('fonte', fonteAtual);

        var btnMenos = document.getElementById('btn-diminuir');
        var btnMais  = document.getElementById('btn-aumentar');
        if (btnMenos) btnMenos.disabled = fonteAtual <= FONTE_MIN;
        if (btnMais)  btnMais.disabled  = fonteAtual >= FONTE_MAX;

        var nomes = { 1: 'Menor', 2: 'Padrão', 3: 'Maior', 4: 'Grande', 5: 'Máximo' };
        anunciar('Tamanho do texto: ' + (nomes[fonteAtual] || fonteAtual));
    }

    window.alterarFonte = function (d) {
        var atual = parseInt(document.documentElement.getAttribute("data-fonte") || fonteAtual, 10);
        fonteAtual = atual;
        aplicarFonte(fonteAtual + d);
    };
    window.resetarFonte  = function ()  { aplicarFonte(FONTE_PADRAO); };

    /* ══ MODO ESCURO ════════════════════════════════════════════ */
    // SVG paths
    var SVG_LUA = 'M12 8.5A5.5 5.5 0 0 1 5.5 2a5.5 5.5 0 1 0 6.5 6.5Z';
    var SVG_SOL = 'M7 4.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5ZM7 1v1.5M7 11.5V13M1 7h1.5M11.5 7H13M2.93 2.93l1.06 1.06M10.01 10.01l1.06 1.06M2.93 11.07l1.06-1.06M10.01 3.99l1.06-1.06';

    function aplicarEscuro(ativo) {
        escuroAtivo = ativo;
        if (ativo) HTML.setAttribute('data-tema', 'escuro');
        else       HTML.removeAttribute('data-tema');
        salvar('escuro', ativo);

        var btn  = document.getElementById('btn-escuro');
        var path = document.getElementById('path-tema');
        if (btn)  btn.setAttribute('aria-pressed', String(ativo));
        if (path) path.setAttribute('d', ativo ? SVG_SOL : SVG_LUA);
        if (btn)  btn.title = ativo ? 'Modo claro' : 'Modo escuro';
        if (btn)  btn.setAttribute('aria-label', ativo ? 'Ativar modo claro' : 'Ativar modo escuro');

        anunciar(ativo ? 'Modo escuro ativado' : 'Modo claro ativado');
    }

    window.alternarModoEscuro = function () { aplicarEscuro(!escuroAtivo); };

    /* ══ ALTO CONTRASTE ═════════════════════════════════════════ */
    function aplicarContraste(ativo) {
        contrasteAtivo = ativo;
        if (ativo) HTML.setAttribute('data-contraste', 'ativo');
        else       HTML.removeAttribute('data-contraste');
        salvar('contraste', ativo);

        var btn = document.getElementById('btn-contraste');
        if (btn) btn.setAttribute('aria-pressed', String(ativo));

        anunciar(ativo ? 'Alto contraste ativado' : 'Alto contraste desativado');
    }

    window.alternarContraste = function () { aplicarContraste(!contrasteAtivo); };

    /* ══ INIT ═══════════════════════════════════════════════════ */
    function init() {
        // Fonte
        fonteAtual = parseInt(carregar('fonte', FONTE_PADRAO), 10);
        aplicarFonte(fonteAtual);

        // Contraste
        contrasteAtivo = carregar('contraste', 'false') === 'true';
        aplicarContraste(contrasteAtivo);

        // Escuro — respeita preferência salva ou do sistema
        var salvoEscuro = carregar('escuro', null);
        if (salvoEscuro !== null) {
            aplicarEscuro(salvoEscuro === 'true');
        } else {
            aplicarEscuro(window.matchMedia('(prefers-color-scheme: dark)').matches);
        }

        // Reage a mudança de tema do sistema (se usuário nunca escolheu)
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function (e) {
            if (carregar('escuro', null) === null) aplicarEscuro(e.matches);
        });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init);
    else init();
})();

/* ── Painel colapsável ───────────────────────────────────────── */
window.toggleAcessibilidade = function () {
    var btn    = document.getElementById('btn-acess-toggle');
    var painel = document.getElementById('acess-painel');
    if (!btn || !painel) return;

    var aberto = btn.getAttribute('aria-expanded') === 'true';

    if (aberto) {
        btn.setAttribute('aria-expanded', 'false');
        btn.setAttribute('aria-label', 'Abrir opções de acessibilidade');
        painel.classList.remove('aberto');
        painel.setAttribute('aria-hidden', 'true');
    } else {
        btn.setAttribute('aria-expanded', 'true');
        btn.setAttribute('aria-label', 'Fechar opções de acessibilidade');
        painel.classList.add('aberto');
        painel.setAttribute('aria-hidden', 'false');
    }
};

// Fecha o painel ao clicar fora
document.addEventListener('click', function (e) {
    var barra = document.getElementById('barra-acessibilidade');
    if (!barra) return;
    if (!barra.contains(e.target)) {
        var btn    = document.getElementById('btn-acess-toggle');
        var painel = document.getElementById('acess-painel');
        if (btn && painel) {
            btn.setAttribute('aria-expanded', 'false');
            painel.classList.remove('aberto');
            painel.setAttribute('aria-hidden', 'true');
        }
    }
});

// Fecha com ESC
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        var btn    = document.getElementById('btn-acess-toggle');
        var painel = document.getElementById('acess-painel');
        if (btn && painel && btn.getAttribute('aria-expanded') === 'true') {
            btn.setAttribute('aria-expanded', 'false');
            painel.classList.remove('aberto');
            painel.setAttribute('aria-hidden', 'true');
            btn.focus();
        }
    }
});