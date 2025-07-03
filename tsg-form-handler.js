// Configuration - CHANGE THIS if your backend runs on different port
const API_CONFIG = {
    baseUrl: 'http://localhost:5000',
    endpoints: {
        submitForm: '/api/forms/upload'  // CHANGED: Use upload endpoint for file support
    }
};

// Initialize when page loads
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('.application-form');
    const submitBtn = document.querySelector('.form-submit');
    
    if (form && submitBtn) {
        form.addEventListener('submit', handleFormSubmission);
        console.log('✅ Form handler loaded successfully');
    } else {
        console.error('❌ Could not find form elements');
    }
});

/**
 * Handle form submission
 */
async function handleFormSubmission(event) {
    event.preventDefault();
    console.log('📝 Form submitted');
    
    const form = event.target;
    const submitBtn = form.querySelector('.form-submit');
    
    // Show loading
    submitBtn.disabled = true;
    submitBtn.textContent = 'Se trimite...';
    
    try {
       // Collect form data with file upload
        const formData = collectFormDataWithFiles(form);
        console.log('📊 Form data collected with files');
        
        // Submit to backend (multipart form data for file upload)
        const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.submitForm}`, {
            method: 'POST',
            mode: 'cors',
            body: formData  // Send FormData directly (no JSON, no Content-Type header)
        });
        
        if (response.ok) {
            // The response should be a PDF file
            const blob = await response.blob();
            
            // Download the PDF
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `TSG_Application_${Date.now()}.pdf`;
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
            
            showSuccessNotification('Aplicația a fost trimisă cu succes!', 'PDF-ul a fost descărcat. Te vom contacta în curând.');
            
            // Redirect to home page after 3 seconds
            setTimeout(() => {
                window.location.href = '../../index.html';
            }, 3000);
        } else {
            const errorText = await response.text();
            console.error('❌ Server error:', errorText);
           showErrorNotification('Eroare la trimiterea aplicației', 'Verificați conexiunea și încercați din nou.');
        }
        
    } catch (error) {
        console.error('❌ Network error:', error);
        showErrorNotification('Eroare de conexiune', 'Verificați dacă backend-ul rulează.');
    } finally {
        // Reset button
        submitBtn.disabled = false;
        submitBtn.textContent = 'Trimite Aplicația';
    }
}

function collectFormDataWithFiles(form) {
    const formData = new FormData();
    
    // Add all text fields - FIXED: Match your HTML field names
    formData.append('name', form.querySelector('[name="first_name"]')?.value || '');
    formData.append('surname', form.querySelector('[name="last_name"]')?.value || '');
    formData.append('email', form.querySelector('[name="email"]')?.value || '');
    formData.append('phone', form.querySelector('[name="phone"]')?.value || '');
    formData.append('birthDate', form.querySelector('[name="birth_date"]')?.value || '');
    formData.append('faculty', form.querySelector('[name="faculty"]')?.value || '');
    formData.append('specialization', form.querySelector('[name="specialization"]')?.value || '');
    formData.append('year', form.querySelector('[name="year"]')?.value || '');
    formData.append('studentId', form.querySelector('[name="student_id"]')?.value || '');
    formData.append('preferredRole', form.querySelector('[name="preferred_role"]')?.value || '');
    formData.append('alternativeRole', form.querySelector('[name="alternative_role"]')?.value || '');
    formData.append('programmingLanguages', form.querySelector('[name="programming_languages"]')?.value || '');
    formData.append('frameworks', form.querySelector('[name="frameworks"]')?.value || '');
    formData.append('tools', form.querySelector('[name="tools"]')?.value || '');
    formData.append('experience', form.querySelector('[name="experience"]')?.value || '');
    formData.append('motivation', form.querySelector('[name="motivation"]')?.value || '');
    formData.append('contribution', form.querySelector('[name="contribution"]')?.value || '');
    formData.append('timeCommitment', form.querySelector('[name="time_commitment"]')?.value || '');
    formData.append('schedule', form.querySelector('[name="schedule"]')?.value || '');
    formData.append('portfolio', form.querySelector('[name="portfolio"]')?.value || '');
    
    // Add checkboxes
    formData.append('dataProcessingAgreement', form.querySelector('[name="data_processing"]')?.checked || false);
    formData.append('termsAgreement', form.querySelector('[name="terms"]')?.checked || false);
    formData.append('newsletterSubscription', form.querySelector('[name="newsletter"]')?.checked || false);
    
    // Add CV file if present - FIXED: Use correct field name 'cv'
    const cvFileInput = form.querySelector('[name="cv"]');
    if (cvFileInput && cvFileInput.files && cvFileInput.files[0]) {
        const cvFile = cvFileInput.files[0];
        formData.append('cv', cvFile);  // CHANGED: from 'cvFile' to 'cv'
        console.log('📎 CV file added to upload:', cvFile.name, 'Size:', cvFile.size);
    }
    
    return formData;
}

/**
 * Show success notification
 */
function showSuccessNotification(title, message) {
    createNotification(title, message, 'success');
}

/**
 * Show error notification  
 */
function showErrorNotification(title, message) {
    createNotification(title, message, 'error');
}

/**
 * Create beautiful animated notification
 */
function createNotification(title, message, type) {
    // Remove any existing notifications
    const existing = document.getElementById('tsg-notification');
    if (existing) {
        existing.remove();
    }
    
    // Create notification element
    const notification = document.createElement('div');
    notification.id = 'tsg-notification';
    notification.className = `tsg-notification tsg-notification-${type}`;
    
    notification.innerHTML = `
        <div class="tsg-notification-content">
            <div class="tsg-notification-icon">
                ${type === 'success' ? '✅' : '❌'}
            </div>
            <div class="tsg-notification-text">
                <div class="tsg-notification-title">${title}</div>
                <div class="tsg-notification-message">${message}</div>
            </div>
            <button class="tsg-notification-close" onclick="closeNotification()">×</button>
        </div>
    `;
    
    // Add to page
    document.body.appendChild(notification);
    
    // Trigger animation
    setTimeout(() => {
        notification.classList.add('tsg-notification-show');
    }, 100);
    
    // Auto-hide after 5 seconds
    setTimeout(() => {
        closeNotification();
    }, 5000);
}

function closeNotification() {
    const notification = document.getElementById('tsg-notification');
    if (notification) {
        notification.classList.remove('tsg-notification-show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }
}

function addLoadingStyles() {
    if (document.getElementById('tsg-form-styles')) return;
    
    const styles = document.createElement('style');
    styles.id = 'tsg-form-styles';
    styles.textContent = `
        /* Beautiful Notifications */
        .tsg-notification {
            position: fixed;
            top: -100px;
            left: 50%;
            transform: translateX(-50%);
            min-width: 400px;
            max-width: 500px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 32px rgba(0, 0, 0, 0.2);
            z-index: 10000;
            opacity: 0;
            transition: all 0.3s ease-out;
            border-left: 6px solid #28a745;
        }
        
        .tsg-notification-error {
            border-left-color: #dc3545;
        }
        
        .tsg-notification-show {
            top: 20px;
            opacity: 1;
        }
        
        .tsg-notification-content {
            display: flex;
            align-items: center;
            padding: 16px 20px;
            gap: 12px;
        }
        
        .tsg-notification-icon {
            font-size: 24px;
            flex-shrink: 0;
        }
        
        .tsg-notification-text {
            flex: 1;
        }
        
        .tsg-notification-title {
            font-weight: bold;
            color: #333;
            margin-bottom: 4px;
        }
        
        .tsg-notification-message {
            color: #666;
            font-size: 14px;
        }
        
        .tsg-notification-close {
            background: none;
            border: none;
            font-size: 24px;
            color: #999;
            cursor: pointer;
            width: 32px;
            height: 32px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: background-color 0.2s;
        }
        
        .tsg-notification-close:hover {
            background-color: #f0f0f0;
            color: #333;
        }
    `;
    
    document.head.appendChild(styles);
}

// Initialize styles when page loads
addLoadingStyles();