(() => {
    const themeKey = "clinic-theme";
    const root = document.documentElement;
    const toggleButton = document.getElementById("themeToggle");

    const setTheme = (theme) => {
        root.setAttribute("data-theme", theme);
        if (toggleButton) {
            toggleButton.innerHTML = theme === "dark"
                ? '<i class="bi bi-sun"></i><span class="d-none d-md-inline">Light mode</span>'
                : '<i class="bi bi-moon-stars"></i><span class="d-none d-md-inline">Dark mode</span>';
            toggleButton.setAttribute("aria-pressed", theme === "dark");
        }
    };

    const savedTheme = localStorage.getItem(themeKey) || "light";
    setTheme(savedTheme);

    if (toggleButton) {
        toggleButton.addEventListener("click", () => {
            const nextTheme = root.getAttribute("data-theme") === "dark" ? "light" : "dark";
            localStorage.setItem(themeKey, nextTheme);
            setTheme(nextTheme);
        });
    }

    document.querySelectorAll("[data-search-table]").forEach((input) => {
        const tableSelector = input.getAttribute("data-search-table");
        const table = tableSelector ? document.querySelector(tableSelector) : null;
        if (!table) return;

        const rows = Array.from(table.querySelectorAll("tbody tr")).filter(
            (row) => !row.hasAttribute("data-empty-row")
        );
        const emptyRow = table.querySelector("tbody tr[data-empty-row]");

        const filterRows = () => {
            const query = input.value.trim().toLowerCase();
            let visibleCount = 0;
            rows.forEach((row) => {
                const matches = row.textContent.toLowerCase().includes(query);
                row.classList.toggle("d-none", !matches);
                if (matches) {
                    visibleCount += 1;
                }
            });

            if (emptyRow) {
                emptyRow.classList.toggle("d-none", visibleCount > 0);
            }
        };

        input.addEventListener("input", filterRows);
    });

    const getThemeValue = (name) =>
        getComputedStyle(root).getPropertyValue(name).trim();

    window.clinicConfirm = (options = {}) => {
        if (!window.Swal) {
            return Promise.resolve({ isConfirmed: window.confirm(options.text || options.title || "Are you sure?") });
        }

        return window.Swal.fire({
            title: options.title || "Are you sure?",
            text: options.text || "This action cannot be undone.",
            icon: options.icon || "warning",
            showCancelButton: true,
            confirmButtonText: options.confirmButtonText || "Yes, continue",
            cancelButtonText: options.cancelButtonText || "Cancel",
            confirmButtonColor: options.confirmButtonColor || getThemeValue("--app-danger"),
            cancelButtonColor: options.cancelButtonColor || getThemeValue("--app-muted"),
            background: getThemeValue("--app-surface"),
            color: getThemeValue("--app-text"),
            reverseButtons: true,
            focusCancel: true,
            customClass: {
                popup: "rounded-4"
            }
        });
    };

    document.querySelectorAll("form[data-confirm]").forEach((form) => {
        form.addEventListener("submit", async (event) => {
            if (form.dataset.confirmed === "true") {
                delete form.dataset.confirmed;
                return;
            }

            event.preventDefault();

            const submitter = event.submitter;
            const result = await window.clinicConfirm({
                title: form.dataset.confirmTitle,
                text: form.dataset.confirmText,
                confirmButtonText: form.dataset.confirmButton || "Yes, continue",
                cancelButtonText: form.dataset.cancelButton || "Cancel"
            });

            if (!result.isConfirmed) {
                return;
            }

            form.dataset.confirmed = "true";

            if (submitter && typeof form.requestSubmit === "function") {
                form.requestSubmit(submitter);
            } else {
                form.submit();
            }
        });
    });

    if (window.bootstrap?.Toast) {
        document.querySelectorAll(".toast").forEach((toast) => {
            const instance = new window.bootstrap.Toast(toast);
            instance.show();
        });
    }
})();
