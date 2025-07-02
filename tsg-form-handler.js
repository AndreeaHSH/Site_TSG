
// Configuration - CHANGE THIS if your backend runs on different port
const API_CONFIG = {
    baseUrl: 'http://localhost:5000',
    endpoints: {
        submitForm: '/api/forms'
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
        // Collect form data
        const formData = collectFormData(form);
        console.log('📊 Form data collected:', formData);
        
        // Submit to backend
        const response = await fetch(`${API_CONFIG.baseUrl}${API_CONFIG.endpoints.submitForm}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            mode: 'cors',
            body: JSON.stringify(formData)
        });
        
         if (response.ok) {
            showSuccessNotification('Aplicația a fost trimisă cu succes!', 'Te vom contacta în curând.');
            
            // Redirect to home page after 2 seconds
            setTimeout(() => {
                window.location.href = '../../index.html';
            }, 2000);
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


function collectFormData(form) {
    const formData = new FormData(form);
    
    return {
        // Personal Information
        name: formData.get('first_name') || '',
        surname: formData.get('last_name') || '',
        email: formData.get('email') || '',
        phone: formData.get('phone') || '',
        birthDate: formData.get('birth_date') || null,
        
        // Academic Information  
        faculty: formData.get('faculty') || '',
        specialization: formData.get('specialization') || '',
        year: formData.get('year') || '',
        studentId: formData.get('student_id') || '',
        
        // Role Preferences
        preferredRole: formData.get('preferred_role') || '',
        alternativeRole: formData.get('alternative_role') || '',
        
        // Technical Skills
        programmingLanguages: formData.get('programming_languages') || '',
        frameworks: formData.get('frameworks') || '',
        tools: formData.get('tools') || '',
        
        // Experience and Motivation
        experience: formData.get('experience') || '',
        motivation: formData.get('motivation') || '',
        contribution: formData.get('contribution') || '',
        
        // Availability
        timeCommitment: formData.get('time_commitment') || '',
        schedule: formData.get('schedule') || '',
        
        // Documents/Portfolio
        portfolio: formData.get('portfolio') || '',
        
        // Agreements
        dataProcessingAgreement: formData.get('data_processing') === 'on',
        termsAgreement: formData.get('terms') === 'on',
        newsletterSubscription: formData.get('newsletter') === 'on'
    };
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

        .success-page {
            opacity: 0;
            transition: opacity 0.5s ease-in;
            text-align: center;
            padding: 3rem 2rem;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 15px;
            color: white;
            margin-bottom: 3rem;
        }
        
        .success-content {
            max-width: 600px;
            margin: 0 auto;
        }
        
        .success-icon {
            font-size: 4rem;
            margin-bottom: 1.5rem;
            animation: bounce 1s ease-in-out infinite alternate;
        }
        
        .success-title {
            font-size: 2.5rem;
            margin-bottom: 1rem;
            font-weight: bold;
        }
        
        .success-message {
            font-size: 1.2rem;
            margin-bottom: 2rem;
            line-height: 1.6;
        }
        
        .success-details {
            background: rgba(255, 255, 255, 0.1);
            padding: 1.5rem;
            border-radius: 10px;
            margin-bottom: 2rem;
            text-align: left;
        }
        
        .success-details ul {
            margin: 1rem 0 0 1rem;
        }
        
        .success-details li {
            margin-bottom: 0.5rem;
        }
        
        .success-actions {
            display: flex;
            gap: 1rem;
            justify-content: center;
            flex-wrap: wrap;
        }
        
        .success-actions .btn {
            padding: 0.75rem 2rem;
            border-radius: 25px;
            text-decoration: none;
            font-weight: bold;
            transition: transform 0.2s ease;
        }
        
        .success-actions .btn:hover {
            transform: translateY(-2px);
        }
        
        .success-actions .btn-primary {
            background: #FF6D48;
            color: white;
            border: none;
        }
        
        .success-actions .btn-secondary {
            background: rgba(255, 255, 255, 0.2);
            color: white;
            border: 2px solid white;
        }
        
        @keyframes bounce {
            from { transform: translateY(0px); }
            to { transform: translateY(-10px); }
        }
    `;
    
    document.head.appendChild(styles);
}

// Initialize styles when page loads
addLoadingStyles();

/**
 * Show success page and hide form
 */
function showSuccessPage() {
    const formContainer = document.querySelector('.application-form-container');
    
    if (formContainer) {
        // Fade out the form
        formContainer.style.transition = 'opacity 0.5s ease-out';
        formContainer.style.opacity = '0';
        
        setTimeout(() => {
            // Hide form and show success message
            formContainer.style.display = 'none';
            
            // Create success page
            const successDiv = document.createElement('div');
            successDiv.className = 'success-page';
            successDiv.innerHTML = `
                <div class="success-content">
                    <div class="success-icon">🎉</div>
                    <h2 class="success-title">Aplicația a fost trimisă cu succes!</h2>
                    <p class="success-message">
                        Mulțumim pentru interesul acordat echipei Transilvania Star Group!<br>
                        Te vom contacta în termen de 7-14 zile cu un răspuns.
                    </p>
                    <div class="success-details">
                        <p><strong>Ce urmează:</strong></p>
                        <ul>
                            <li>Verificăm aplicația ta</li>
                            <li>Te contactăm pentru un interviu</li>
                            <li>Îți trimitem detalii despre echipă</li>
                        </ul>
                    </div>
                    <div class="success-actions">
                        <a href="../../index.html" class="btn btn-primary">
                            Înapoi la Pagina Principală
                        </a>
                        <button onclick="location.reload()" class="btn btn-secondary">
                            Aplică Din Nou
                        </button>
                    </div>
                </div>
            `;
            
            // Insert success page
            formContainer.parentNode.insertBefore(successDiv, formContainer);
            
            // Fade in success page
            setTimeout(() => {
                successDiv.style.opacity = '1';
            }, 100);
        }, 500);
    }
}