

export function IsValidEmail(email:string):boolean {

    const pattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    return email.trim() != '' && pattern.test(email);
}

export function IsValidPassword(password:string):boolean {

    // Regular expression to check for at least one uppercase letter and one symbol
    const regex = /^(?=.*[A-Z])(?=.*[!@#$%^&*(),.?":{}|<>]).+$/;

    // Test the password against the regex
    return regex.test(password);
}