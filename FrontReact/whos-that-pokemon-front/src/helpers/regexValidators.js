export const hasEnoughUpperLowerChars = (password) => {
    const upperChars = (password.match(/[A-Z]/g) || []).length;
    const lowerChars = (password.match(/[a-z]/g) || []).length;
    return upperChars >= 1 && lowerChars >= 2;
}

export const hasEnoughChars = (password) => {
    return password.length > 8 && password.length < 15;
}

export const areSame = (password1, password2) => {
    return password1 === password2;
}

export const hasEnoughSpecialChars = (password) => {
    const specialChars = (password.match(/[.*+!?^${}()|[\]\\]/g) || []).length;
    return specialChars >= 1;
}

export const hasEnoughDigits = (password) => {
    const digits = (password.match(/[0-9]/g) || []).length;
    return digits >= 2;
}