/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./**/*.{razor,cshtml,html,js}",
        'node_modules/preline/dist/*.js',
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('daisyui'),
        require('preline/plugin'),
    ],
}

