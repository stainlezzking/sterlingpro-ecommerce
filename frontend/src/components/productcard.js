import React from "react";

const ProductCard = ({ product }) => {
  const formatPrice = (price) => {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
    }).format(price);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString();
  };

  const handleAddToCart = () => {
    fetch(process.env.REACT_APP_BACKEND_URL + "/api/cart", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify({ productId: product.id, quantity: 1 }),
    })
      .then((response) => {
        if (!response.ok) {
          throw new Error("Failed to add item to cart");
        }
        return response.json();
      })
      .then((data) => {
        console.log("Item added to cart:", data);
        // you can update the cart state here
      })
      .catch((error) => {
        alert(error.message);
        console.error("Error adding item to cart:", error);
      });
  };
  return (
    <div className="product-card">
      <div className="product-header">
        <h3 className="product-name">{product.name}</h3>
        <button className="" onClick={handleAddToCart} disabled={!product.isActive || product.stockQuantity === 0}>
          {product.stockQuantity === 0 ? "Out of Stock" : "Add to Cart"}
        </button>
      </div>

      <div className="product-details">
        <p className="product-description">{product.description}</p>

        <div className="product-info">
          <div className="price-stock">
            <span className="product-price">{formatPrice(product.price)}</span>
            <span className="product-stock">Stock: {product.stockQuantity}</span>
          </div>

          <div className="product-category">
            <span className="category-tag">{product.category}</span>
          </div>

          <div className="product-dates">
            <small>Created: {formatDate(product.createdAt)}</small>
            <small>Updated: {formatDate(product.updatedAt)}</small>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
