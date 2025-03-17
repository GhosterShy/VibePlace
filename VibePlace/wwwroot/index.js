const scrollableElement = document.querySelector('.filter-contents-home'); // Замените на ваш элемент

let isDragging = false;
let startX;
let scrollLeft;

scrollableElement.addEventListener('mousedown', (e) => {
  isDragging = true;
  startX = e.pageX - scrollableElement.offsetLeft;
  scrollLeft = scrollableElement.scrollLeft;
});

scrollableElement.addEventListener('mouseleave', () => {
    isDragging = false;
});

scrollableElement.addEventListener('mouseup', () => {
  isDragging = false;
});

scrollableElement.addEventListener('mousemove', (e) => {
  if (!isDragging) return;
  e.preventDefault();
  const x = e.pageX - scrollableElement.offsetLeft;
  const walk = (x - startX) * 1; // Чувствительность прокрутки
  scrollableElement.scrollLeft = scrollLeft - walk;
});




let currentPage = 1;
  const itemsPerPage = 8; // 8 items per page
  const gridItems = document.querySelectorAll('.grid-item-home');
  const totalPages = Math.ceil(gridItems.length / itemsPerPage);

  const updatePagination = () => {
    // Hide all items
    gridItems.forEach((item, index) => {
      item.style.display = 'none';
      if (index >= (currentPage - 1) * itemsPerPage && index < currentPage * itemsPerPage) {
        item.style.display = 'block';
      }
    });
    
    // Update the page buttons
    document.querySelectorAll('.page-btn').forEach(button => {
      button.classList.remove('bg-indigo-600', 'text-white');
      button.classList.add('text-gray-500', 'hover:text-indigo-900');
      if (parseInt(button.getAttribute('data-page')) === currentPage) {
        button.classList.add('bg-indigo-600', 'text-white');
      }
    });

    // Disable/enable next/prev buttons
    document.getElementById('prev-btn').style.pointerEvents = currentPage === 1 ? 'none' : 'auto';
    document.getElementById('next-btn').style.pointerEvents = currentPage === totalPages ? 'none' : 'auto';
  };

  document.querySelectorAll('.page-btn').forEach(button => {
    button.addEventListener('click', (e) => {
      currentPage = parseInt(e.target.getAttribute('data-page'));
      updatePagination();
    });
  });

  document.getElementById('prev-btn').addEventListener('click', () => {
    if (currentPage > 1) {
      currentPage--;
      updatePagination();
    }
  });

  document.getElementById('next-btn').addEventListener('click', () => {
    if (currentPage < totalPages) {
      currentPage++;
      updatePagination();
    }
  });

  // Initialize pagination
  updatePagination();




