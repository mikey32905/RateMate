// netlify/functions/exchange-rate.js
//const fetch = require('node-fetch');

exports.handler = async function (event, context) {
    // Enable CORS
    const headers = {
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Headers': 'Content-Type',
        'Access-Control-Allow-Methods': 'GET, OPTIONS',
        'Content-Type': 'application/json'
    };

    // Handle preflight requests
    if (event.httpMethod === 'OPTIONS') {
        return {
            statusCode: 200,
            headers,
            body: ''
        };
    }

    try {
        // Get parameters from query string
        const fromCurrency = event.queryStringParameters.from || 'USD';
        const toCurrency = event.queryStringParameters.to || 'EUR';
        const amount = event.queryStringParameters.amount || '1';

        // Your API key is stored securely in environment variables
        const apiKey = process.env.EXCHANGE_RATE_API_KEY;

        // Build the API URL using the pair endpoint
        const apiUrl = `https://v6.exchangerate-api.com/v6/${apiKey}/pair/${fromCurrency}/${toCurrency}/${amount}`;

        const response = await fetch(apiUrl);
        const data = await response.json();

        if (!response.ok || data.result === 'error') {
            throw new Error(data['error-type'] || `API responded with status: ${response.status}`);
        }

        // Return the conversion data
        const responseData = {
            base_code: data.base_code,
            target_code: data.target_code,
            conversion_rate: data.conversion_rate,
            conversion_result: data.conversion_result,
            time_last_update_unix: data.time_last_update_unix
        };

        return {
            statusCode: 200,
            headers,
            body: JSON.stringify(responseData)
        };

    } catch (error) {
        console.error('Error fetching exchange rates:', error);

        return {
            statusCode: 500,
            headers,
            body: JSON.stringify({
                error: 'Failed to fetch exchange rates',
                message: error.message
            })
        };
    }
};