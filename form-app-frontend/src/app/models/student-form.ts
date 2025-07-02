export interface StudentForm {
  id?: number;

  // Personal Information
  name: string;
  surname: string;
  email?: string;
  phone?: string;
  birthDate?: string;

  // Academic Information
  faculty: string;
  specialization?: string;
  year?: string;
  studentId?: string;

  // Role Preferences
  preferredRole?: string;
  alternativeRole?: string;

  // Technical Skills
  programmingLanguages?: string;
  frameworks?: string;
  tools?: string;

  // Experience and Motivation
  experience?: string;
  motivation: string;
  contribution?: string;

  // Availability
  timeCommitment?: string;
  schedule?: string;

  // Documents/Portfolio
  portfolio?: string;
  cvFileName?: string;
  cvFilePath?: string;

  // Agreements
  dataProcessingAgreement?: boolean;
  termsAgreement?: boolean;
  newsletterSubscription?: boolean;

  // System fields
  submissionDate?: string;
}
