module.exports = {
  root: true,
  env: {
    browser: true,
    node: true
  },
  parserOptions: {
    parser: '@babel/eslint-parser',
    requireConfigFile: false
  },
  extends: [
    '@nuxtjs',
    'plugin:nuxt/recommended'
  ],
  plugins: [
  ],
  // add your custom rules here
  rules: {
    "space-before-function-paren": ["off", "always"],
    "semi": "off",
    "indent": "off",
    "arrow-spacing": "off",
    "quotes": "off",
    "vue/max-attributes-per-line": "off",
    "space-before-blocks": "off",
    "object-curly-spacing": "off"
  }
}
