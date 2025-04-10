// Common JavaScript functions for the site

// Show a toast notification
function showToast(message, type = 'success') {
    // This is a placeholder for a toast notification system
    // In a real application, you would use a library like Toastr or Bootstrap's toast component
    alert(message);
}

// Format a date string
function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
}

// Handle AJAX errors
function handleAjaxError(xhr, status, error) {
    console.error('AJAX Error:', status, error);
    
    if (xhr.status === 401) {
        showToast('Your session has expired. Please log in again.', 'error');
        window.location.href = '/Account/Login';
        return;
    }
    
    if (xhr.status === 403) {
        showToast('You do not have permission to perform this action.', 'error');
        return;
    }
    
    if (xhr.responseJSON && xhr.responseJSON.message) {
        showToast(xhr.responseJSON.message, 'error');
    } else {
        showToast('An error occurred. Please try again.', 'error');
    }
}
