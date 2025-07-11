// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Dark Mode Toggle Functionality
document.addEventListener('DOMContentLoaded', function() {
    const themeToggle = document.getElementById('themeToggle');
    const themeIcon = document.getElementById('themeIcon');
    const body = document.body;
    
    // Check for saved theme preference or default to 'light'
    const currentTheme = localStorage.getItem('theme') || 'light';
    
    // Set initial theme
    if (currentTheme === 'dark') {
        body.setAttribute('data-theme', 'dark');
        themeIcon.className = 'fas fa-sun';
    } else {
        body.setAttribute('data-theme', 'light');
        themeIcon.className = 'fas fa-moon';
    }
    
    // Theme toggle event listener
    themeToggle.addEventListener('click', function() {
        const currentTheme = body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        body.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        
        // Update icon
        if (newTheme === 'dark') {
            themeIcon.className = 'fas fa-sun';
        } else {
            themeIcon.className = 'fas fa-moon';
        }
    });
});

// Task Sorting Functionality
function sortTable(columnIndex, tableId) {
    const table = document.getElementById(tableId);
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    
    // Determine sort direction
    const currentSort = table.getAttribute('data-sort');
    const isAscending = currentSort !== `${columnIndex}-asc`;
    
    rows.sort((a, b) => {
        let aValue = a.cells[columnIndex].textContent.trim();
        let bValue = b.cells[columnIndex].textContent.trim();
        
        // Handle different data types
        if (columnIndex === 1) { // Due Date
            aValue = new Date(aValue);
            bValue = new Date(bValue);
        } else if (columnIndex === 2) { // Priority
            const priorityOrder = { 'High': 3, 'Medium': 2, 'Low': 1 };
            aValue = priorityOrder[aValue] || 0;
            bValue = priorityOrder[bValue] || 0;
        }
        
        if (aValue < bValue) return isAscending ? -1 : 1;
        if (aValue > bValue) return isAscending ? 1 : -1;
        return 0;
    });
    
    // Update table
    rows.forEach(row => tbody.appendChild(row));
    
    // Update sort indicator
    table.setAttribute('data-sort', `${columnIndex}-${isAscending ? 'asc' : 'desc'}`);
    
    // Update header indicators
    updateSortIndicators(table, columnIndex, isAscending);
}

function updateSortIndicators(table, activeColumn, isAscending) {
    const headers = table.querySelectorAll('th');
    headers.forEach((header, index) => {
        const icon = header.querySelector('.sort-icon');
        if (icon) {
            if (index === activeColumn) {
                icon.className = `fas fa-sort-${isAscending ? 'up' : 'down'} sort-icon ms-1`;
            } else {
                icon.className = 'fas fa-sort sort-icon ms-1';
            }
        }
    });
}

// Search functionality
function filterTasks(searchTerm, tableId) {
    const table = document.getElementById(tableId);
    const tbody = table.querySelector('tbody');
    const rows = tbody.querySelectorAll('tr');
    
    searchTerm = searchTerm.toLowerCase();
    
    rows.forEach(row => {
        const taskName = row.cells[0].textContent.toLowerCase();
        const category = row.cells[3].textContent.toLowerCase();
        
        if (taskName.includes(searchTerm) || category.includes(searchTerm)) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}
